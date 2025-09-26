using Timesheet.API.Entities;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IUserAccountRepository
    {
        //UserAccountModel CreateUserAccount(CreateUserAccountDto userAccountDto);
        //void DeleteUserAccount(UserAccountModel userAccount);
        //UserAccountModel? FindByEmployeeIdNumber(int employeeIdNumber);
        //List<UserAccountModel> GetAllUserAccounts();
        Task<int> DeleteUserAccountsByEmployeeIdAsync(int id);
        Task<(UserAccountModel?, UserAccount?)> CreateUserAccountAsync(CreateUserAccountDto userAccountDto);
        Task<IEnumerable<UserAccountModel>> GetUserAccountsAsync();
    }
}
