using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<ServiceResult<UserAccount>> CreateUserAccount(CreateUserAccountDto userAccountDto);
        Task<ServiceResult<IEnumerable<UserAccount>>> GetUserAccounts();
        //Task<int> DeleteUserAccount(int id);
        //Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId);
        Task<ServiceResult<UserAccount>> DeleteUserAccount(int userAccountId);
        Task<ServiceResult<UserAccount>> GetUserAccountById(int userAccountId);
    }
}