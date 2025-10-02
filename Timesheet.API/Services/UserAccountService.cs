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

        public async Task<UserAccountModel?> CreateUserAccountAsync(CreateUserAccountDto userAccountDto)
        {
            //return await _userAccountRepository.CreateUserAccountAsync(userAccountDto);

            var employeeModelFound = await _employeeService.GetEmployeeByIdAsync(userAccountDto.EmployeeId);

            if (employeeModelFound == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(userAccountDto.Email) || string.IsNullOrEmpty(userAccountDto.Password))
            {
                userAccountDto.Email = $"{employeeModelFound.FirstName.ToLower()}.{employeeModelFound.LastName.ToLower()}@company.com";
                userAccountDto.Password = "DefaultPassword123";
            }

            var (newUserAccountModel, newUserAccountEntity) = await _userAccountRepository.CreateUserAccountAsync(userAccountDto);

            if (newUserAccountModel == null || newUserAccountEntity == null)
                return null;

            await _employeeService.UpdateEmployeeUserAccountsAsync(newUserAccountEntity);

            return newUserAccountModel;
        }

        public async Task<IEnumerable<UserAccountModel>> GetUserAccountsAsync()
        {
            return await _userAccountRepository.GetUserAccountsAsync();
        }

        public async Task<int> DeleteUserAccountAsync(int id)
        {
            return await _userAccountRepository.DeleteUserAccountsByEmployeeIdAsync(id);
        }
    }
}
