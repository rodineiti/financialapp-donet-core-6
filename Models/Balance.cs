using Microsoft.AspNetCore.Identity;

namespace FinancialAppMvc.Models;

public class Balance
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }
    public decimal CurrentBalance { get; set; }
}
