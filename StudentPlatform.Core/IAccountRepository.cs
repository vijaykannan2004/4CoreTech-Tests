using System.Threading.Tasks;

namespace StudentPlatform.Core
{
    public interface IAccountRepository
    {
        Task<bool> IsEmailUnique(string email);
        Task<bool> ValidateCredentials(string email, string password);
        Task LockAccount(string email);
        Task<bool> IsAccountLocked(string email);
    }
}
