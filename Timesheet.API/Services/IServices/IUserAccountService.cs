using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        UserAccount? CreateUserAccount(CreateUserAccountDto userAccountDto);
        bool DeleteUserAccount(int employeeIdNumber);
        UserAccount? FindByEmployeeIdNumber(int id);
        List<UserAccount> GetUserAccountMockData();
        List<UserAccount> GetUserAccounts();
    }
}