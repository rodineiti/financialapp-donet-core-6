namespace FinancialAppMvc.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal AmountChanged { get; set; }
    public decimal NewBalance { get; set; }
    public DateTime Date { get; set; }
    public int? TransactionId { get; set; }
}
