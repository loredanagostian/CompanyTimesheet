using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IUserAccountRepository
    {
        Task CreateUserAccount(UserAccount userAccount);
        Task<ServiceResult<IEnumerable<UserAccount>>> GetUserAccounts();
        Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId);
        Task DeleteUserAccount(UserAccount userAccount);
        Task<UserAccount?> GetUserAccountById(int id);
    }
}
