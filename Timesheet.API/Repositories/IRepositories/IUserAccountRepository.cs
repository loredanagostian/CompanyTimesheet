using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IUserAccountRepository
    {
        Task<int> DeleteUserAccountsByEmployeeIdAsync(int id);
        Task<UserAccount?> CreateUserAccountAsync(CreateUserAccountDto userAccountDto);
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();
    }
}
