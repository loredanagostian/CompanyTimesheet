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

        public UserAccount? CreateUserAccount(CreateUserAccountDto userAccountDto)
        {
            var linkedEmployee = _employeeService.FindByEmployeeIdNumber(userAccountDto.EmployeeId);

            if (linkedEmployee == null)
            {
                return null;
            }

            var newUserAccount = _userAccountRepository.CreateUserAccount(linkedEmployee);

            _employeeService.UpdateEmployeeUserAccounts(newUserAccount);

            return newUserAccount;
        }

        public List<UserAccount> GetUserAccountMockData()
        {
            List<UserAccount> _users = new List<UserAccount>();
            var _employees = _employeeService.GetEmployeesMockData();

            foreach (var employee in _employees)
            {
                var newUserAccount = CreateUserAccount(
                    new CreateUserAccountDto
                    {
                        EmployeeId = employee.EmployeeId
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

        public List<UserAccount> GetUserAccounts()
        {
            return _userAccountRepository.GetAllUserAccounts();
        }

        public UserAccount? FindByEmployeeIdNumber(int id)
        {
            return _userAccountRepository.FindByEmployeeIdNumber(id);
        }
    }
}
