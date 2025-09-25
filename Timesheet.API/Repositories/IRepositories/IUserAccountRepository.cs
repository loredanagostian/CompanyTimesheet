using Timesheet.API.Models;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IUserAccountRepository
    {
        UserAccount CreateUserAccount(Employee employee);
        void DeleteUserAccount(UserAccount userAccount);
        UserAccount? FindByEmployeeIdNumber(int employeeIdNumber);
        List<UserAccount> GetAllUserAccounts();
    }
}
