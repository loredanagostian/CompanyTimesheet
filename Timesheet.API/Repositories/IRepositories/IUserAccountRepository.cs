using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IUserAccountRepository
    {
        Task<int> DeleteUserAccountsByEmployeeIdAsync(int id);
        Task<UserAccount?> CreateUserAccount(CreateUserAccountDto userAccountDto, Employee employee);
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();
        Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId);
        Task DeleteUserAccount(UserAccount userAccount);
    }
}
