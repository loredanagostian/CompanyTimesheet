using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        UserAccountModel? CreateUserAccount(CreateUserAccountDto userAccountDto);
        bool DeleteUserAccount(int employeeIdNumber);
        UserAccountModel? FindByEmployeeIdNumber(int id);
        List<UserAccountModel> GetUserAccountMockData();
        List<UserAccountModel> GetUserAccounts();
    }
}