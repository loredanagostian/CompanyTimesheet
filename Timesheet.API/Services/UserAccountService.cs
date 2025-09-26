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

        public UserAccountModel? CreateUserAccount(CreateUserAccountDto userAccountDto)
        {
            var linkedEmployee = _employeeService.FindByEmployeeIdNumber(userAccountDto.EmployeeId);

            if (linkedEmployee == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(userAccountDto.Email) || string.IsNullOrEmpty(userAccountDto.Password))
            {
                userAccountDto.Email = $"{linkedEmployee.FirstName.ToLower()}.{linkedEmployee.LastName.ToLower()}@company.com";
                userAccountDto.Password = "DefaultPassword123";
            }

            var newUserAccount = _userAccountRepository.CreateUserAccount(userAccountDto);

            _employeeService.UpdateEmployeeUserAccounts(newUserAccount);

            return newUserAccount;
        }

        public List<UserAccountModel> GetUserAccountMockData()
        {
            List<UserAccountModel> _users = new List<UserAccountModel>();
            var _employees = _employeeService.GetEmployeesMockData();

            foreach (var employee in _employees)
            {
                var newUserAccount = CreateUserAccount(
                    new CreateUserAccountDto
                    {
                        EmployeeId = employee.EmployeeId,
                        Email = $"{employee.FirstName.ToLower()}.{employee.LastName.ToLower()}@company.com",
                        Password = "DefaultPassword123"
                    }
                );

                if (newUserAccount != null)
                    _users.Add(newUserAccount);
            }

            return _users;
        }

        public bool DeleteUserAccount(int employeeIdNumber)
        {
            var userAccountFound = _userAccountRepository.FindByEmployeeIdNumber(employeeIdNumber);

            if (userAccountFound == null)
                return false;

            var linkedEmployee = _employeeService.FindByEmployeeIdNumber(employeeIdNumber);

            linkedEmployee!.UserAccounts.Remove(userAccountFound);

            _userAccountRepository.DeleteUserAccount(userAccountFound);

            return true;
        }

        public List<UserAccountModel> GetUserAccounts()
        {
            return _userAccountRepository.GetAllUserAccounts();
        }

        public UserAccountModel? FindByEmployeeIdNumber(int id)
        {
            return _userAccountRepository.FindByEmployeeIdNumber(id);
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
