using FinancialAppMvc.Repositories;

namespace FinancialAppMvc.Actions
{
    public class UserAction
    {
        private readonly UserRepository _userRepository;

        public UserAction(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<decimal> GetBalanceAsync()
        {
            return await _userRepository.GetBalanceAsync();
        }

    }
}