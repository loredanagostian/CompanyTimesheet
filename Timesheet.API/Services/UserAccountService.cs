using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class UserAccountService : IUserAccountService
    {
        private static List<UserAccount> _userAccounts = new List<UserAccount>();
        private readonly IEmployeeService _employeeService;

        public UserAccountService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public UserAccount? CreateUserAccount(CreateUserAccountDto userAccountDto)
        {
            var linkedEmployee = _employeeService.FindByEmployeeIdNumber(userAccountDto.EmployeeIdNumber);

            if (linkedEmployee == null)
            {
                return null;
            }

            var userAccount = new UserAccount
            {
                UserId = Guid.NewGuid(),
                Email = $"{linkedEmployee.FirstName.ToLower()}.{linkedEmployee.LastName.ToLower()}@company.com",
                Password = "something",
                Employee = linkedEmployee
            };

            _userAccounts.Add(userAccount);

            return userAccount;
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
                        EmployeeIdNumber = employee.EmployeeIdNumber
                    }
                );

                if (newUserAccount != null)
                    _users.Add(newUserAccount);
            }

            return _users;
        }

        public bool DeleteUserAccount(int employeeIdNumber)
        {
            var userAccountFound = _userAccounts.FirstOrDefault(u => u.Employee.EmployeeIdNumber == employeeIdNumber);

            if (userAccountFound == null)
                return false;

            _userAccounts.Remove(userAccountFound);
            return true;
        }

        public List<UserAccount> GetUserAccounts()
        {
            return _userAccounts;
        }

        public UserAccount? FindByEmployeeIdNumber(int id)
        {
            return _userAccounts.FirstOrDefault(u => u.Employee.EmployeeIdNumber == id);
        }
    }
}
