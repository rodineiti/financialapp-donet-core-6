using FinancialAppMvc.Contracts;
using Microsoft.AspNetCore.Identity;

namespace FinancialAppMvc.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserContextService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string? GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            return user != null ? _userManager.GetUserId(user) : null;
        }
    }
}