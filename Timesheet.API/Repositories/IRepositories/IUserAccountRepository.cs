using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IUserAccountRepository
    {
        Task<int> DeleteUserAccountsByEmployeeIdAsync(int id);
        Task CreateUserAccount(UserAccount userAccount);
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();
        Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId);
        Task DeleteUserAccount(UserAccount userAccount);
    }
}
