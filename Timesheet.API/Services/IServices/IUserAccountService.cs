using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccount?> CreateUserAccountAsync(CreateUserAccountDto userAccountDto);
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();
        Task<int> DeleteUserAccountAsync(int id);
    }
}