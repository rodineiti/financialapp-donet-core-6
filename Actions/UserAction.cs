using FinancialAppMvc.Contracts;
using FinancialAppMvc.Repositories;

namespace FinancialAppMvc.Actions
{
    public class UserAction: BaseAction
    {
        private readonly UserRepository _userRepository;

        public UserAction(UserRepository userRepository, IUserContextService userContextService) : base(userContextService)
        {
            _userRepository = userRepository;
        }

        public async Task<decimal> GetBalanceAsync()
        {
            return await _userRepository.GetBalanceAsync(UserId);
        }

    }
}