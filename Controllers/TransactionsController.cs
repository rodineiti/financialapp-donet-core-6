using Microsoft.AspNetCore.Mvc;
using FinancialAppMvc.Models;
using FinancialAppMvc.Contracts;
using Microsoft.AspNetCore.Authorization;
using FinancialAppMvc.Actions;

namespace FinancialAppMvc.Controllers;

[Authorize]
public class TransactionsController : Controller
{
    private readonly ILogger<TransactionsController> _logger;
    private readonly ITransactionAction _transactionAction;
    private readonly IUserContextService _userContextService;
    private readonly UserAction _userAction;

    public TransactionsController(ILogger<TransactionsController> logger, ITransactionAction transactionAction, IUserContextService userContextService, UserAction userAction)
    {
        _logger = logger;
        _transactionAction = transactionAction;
        _userContextService = userContextService;
        _userAction = userAction;
    }

    public async Task<IActionResult> Index()
    {
        var transactions = await _transactionAction.GetAllAsync();
        var balance = await _userAction.GetBalanceAsync();

        ViewBag.Balance = balance;

        return View(transactions);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Store(Transaction transaction)
    {
        transaction.UserId = _userContextService.GetUserId();

        if (ModelState.IsValid)
        {
            await _transactionAction.StoreAsync(transaction);

            return RedirectToAction(nameof(Index));
        }

        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            ModelState.AddModelError("", error.ErrorMessage);
        }

        return View("Create", transaction);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var transaction = await _transactionAction.GetByIdAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return NotFound();
        }

        transaction.UserId = _userContextService.GetUserId();

        if (ModelState.IsValid)
        {
            await _transactionAction.UpdateAsync(transaction);

            return RedirectToAction(nameof(Index));
        }

        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            ModelState.AddModelError("", error.ErrorMessage);
        }

        return View("Edit", transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Destroy(int id)
    {
        await _transactionAction.DestroyAsync(id);
        
        return RedirectToAction(nameof(Index));
    }
}
