using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccount?> CreateUserAccount(CreateUserAccountDto userAccountDto, Employee employee);
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();
        Task<int> DeleteUserAccountAsync(int id);
        Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId);
        Task DeleteUserAccount(UserAccount userAccount);
    }
}