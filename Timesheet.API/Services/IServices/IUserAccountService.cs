using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<ServiceResult<UserAccount>> CreateUserAccount(CreateUserAccountDto userAccountDto);
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();
        Task<int> DeleteUserAccountAsync(int id);
        Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId);
        Task DeleteUserAccount(UserAccount userAccount);
    }
}