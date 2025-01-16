using FinancialAppMvc.Contracts;

namespace FinancialAppMvc.Actions
{
    public abstract class BaseAction
    {
        private readonly IUserContextService _userContextService;

        public BaseAction(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        public string? UserId => _userContextService.GetUserId();
    }
}