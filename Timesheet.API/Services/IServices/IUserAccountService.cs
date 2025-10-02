using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserAccountModel?> CreateUserAccountAsync(CreateUserAccountDto userAccountDto);
        Task<IEnumerable<UserAccountModel>> GetUserAccountsAsync();
        Task<int> DeleteUserAccountAsync(int id);
    }
}