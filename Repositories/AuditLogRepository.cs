using FinancialAppMvc.Contracts;
using FinancialAppMvc.Data;
using FinancialAppMvc.Models;

namespace FinancialAppMvc.Repositories
{
    public class AuditLogRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserContextService _userContextService;

        public AuditLogRepository(AppDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task AddAuditLogAsync(AuditLog auditLog)
        {
            auditLog.UserId = _userContextService.GetUserId();

            _context.AuditLogs.Add(auditLog);
            
            await _context.SaveChangesAsync();
        }
    }
}