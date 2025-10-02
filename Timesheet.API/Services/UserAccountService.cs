using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserAccountRepository _userAccountRepository;

        public UserAccountService(IEmployeeService employeeService, IUserAccountRepository userAccountRepository)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
        }

        public async Task<UserAccount?> CreateUserAccount(CreateUserAccountDto userAccountDto, Employee employee)
        {
            var newUserAccountModel = await _userAccountRepository.CreateUserAccount(userAccountDto, employee);

            if (newUserAccountModel == null)
                return null;

            await _employeeService.UpdateEmployeeUserAccountsAsync(newUserAccountModel);

            return newUserAccountModel;
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccountsAsync()
        {
            return await _userAccountRepository.GetUserAccountsAsync();
        }

        public async Task<int> DeleteUserAccountAsync(int id)
        {
            return await _userAccountRepository.DeleteUserAccountsByEmployeeIdAsync(id);
        }

        public async Task DeleteUserAccount(UserAccount userAccount)
        {
            await _userAccountRepository.DeleteUserAccount(userAccount);
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId)
        {
            return await _userAccountRepository.GetUserAccountsByEmployeeId(employeeId);
        }
    }
}
