using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private static List<UserAccount> _userAccounts = new List<UserAccount>();

        public UserAccount CreateUserAccount(Employee employee)
        {
            var userAccount = new UserAccount
            {
                Email = $"{employee.FirstName.ToLower()}.{employee.LastName.ToLower()}@company.com",
                Password = "something",
                EmployeeId = employee.EmployeeId
            };

            _userAccounts.Add(userAccount);

            return userAccount;
        }

        public void DeleteUserAccount(UserAccount userAccount)
        {
            _userAccounts.Remove(userAccount);
        }

        public UserAccount? FindByEmployeeIdNumber(int employeeIdNumber)
        {
            return _userAccounts.FirstOrDefault(u => u.EmployeeId == employeeIdNumber);
        }

        public List<UserAccount> GetAllUserAccounts()
        {
            return _userAccounts;
        }
    }
}
