using System.Linq;
using System.Threading.Tasks;

namespace StudentPlatform.Core
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class AccountService
    {
        private readonly IAccountRepository _repo;

        public AccountService(IAccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResult> CreateAccount(Account account)
        {
            if (!await _repo.IsEmailUnique(account.Email))
                return new ServiceResult { Success = false, Message = "Email already exists" };

            if (!ValidatePassword(account.Password))
                return new ServiceResult { Success = false, Message = "Weak password" };

            return new ServiceResult { Success = true, Message = "Account created" };
        }

        public bool ValidatePassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit);
        }

        public async Task<ServiceResult> Login(string email, string password)
        {
            if (await _repo.IsAccountLocked(email))
                return new ServiceResult { Success = false, Message = "Account locked" };

            if (!await _repo.ValidateCredentials(email, password))
            {
                await _repo.LockAccount(email);
                return new ServiceResult { Success = false, Message = "Invalid credentials" };
            }

            return new ServiceResult { Success = true, Message = "Login successful" };
        }

        public async Task<bool> IsAccountLocked(string email)
        {
            return await _repo.IsAccountLocked(email);
        }
    }
}
