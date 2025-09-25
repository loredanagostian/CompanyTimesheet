using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private static List<UserAccountModel> _userAccounts = new List<UserAccountModel>();

        public UserAccountModel CreateUserAccount(CreateUserAccountDto userAccountDto)
        {
            var userAccount = new UserAccountModel
            {
                Email = userAccountDto.Email,
                Password = userAccountDto.Password,
                EmployeeId = userAccountDto.EmployeeId
            };

            _userAccounts.Add(userAccount);

            return userAccount;
        }

        public void DeleteUserAccount(UserAccountModel userAccount)
        {
            _userAccounts.Remove(userAccount);
        }

        public UserAccountModel? FindByEmployeeIdNumber(int employeeIdNumber)
        {
            return _userAccounts.FirstOrDefault(u => u.EmployeeId == employeeIdNumber);
        }

        public List<UserAccountModel> GetAllUserAccounts()
        {
            return _userAccounts;
        }
    }
}
