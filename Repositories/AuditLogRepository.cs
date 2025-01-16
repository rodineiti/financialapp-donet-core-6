using FinancialAppMvc.Data;
using FinancialAppMvc.Models;

namespace FinancialAppMvc.Repositories
{
    public class AuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAuditLogAsync(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            
            await _context.SaveChangesAsync();
        }
    }
}