using System.ComponentModel.DataAnnotations;
using FinancialAppMvc.Enums;
using Microsoft.AspNetCore.Identity;

namespace FinancialAppMvc.Models;

public class Transaction
{
    public int Id { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    public string? Category { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [ScaffoldColumn(false)]
    public string? UserId { get; set; }

    public IdentityUser? User { get; set; }
}
