using System.Net;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;
using Timesheet.API.Validations;

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

        private string ComputeEmail(Employee employee)
        {
            var firstNamePart = employee.FirstName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
            var lastNamePart = employee.LastName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();

            return $"{firstNamePart}.{lastNamePart}@company.com";
        }

        public async Task<ServiceResult<UserAccount>> CreateUserAccount(CreateUserAccountDto userAccountDto)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(userAccountDto.EmployeeId);

            if (employee == null)
                return ServiceResult<UserAccount>.Failure(
                    $"No Employee was found with ID {userAccountDto.EmployeeId}."
                );

            var errors = UserAccountValidation.Validate(userAccountDto);

            if (errors.Count > 0)
                return ServiceResult<UserAccount>.ValidationFailure(errors);

            var userAccount = await _userAccountRepository.GetUserAccountsByEmployeeId(userAccountDto.EmployeeId);

            if (userAccount.Any(ua => ua.Email.Equals(userAccountDto.Email)))
                return ServiceResult<UserAccount>.Failure(
                    $"An User Account already exists for Employee with ID {userAccountDto.EmployeeId} and Email {userAccountDto.Email}."
                );

            var newUserAccount = new UserAccount
            {
                EmployeeId = userAccountDto.EmployeeId,
                Email = userAccountDto.Email ?? ComputeEmail(employee),
                Password = userAccountDto.Password ?? "P4$$W0Rd", // Temporary password if not provided
                HasDefaultPassword = userAccountDto.Password == null,
                IsAlias = userAccountDto.Email != null
            };

            await _userAccountRepository.CreateUserAccount(newUserAccount);

            await _employeeService.AddEmployeeUserAccount(employee, newUserAccount);

            return ServiceResult<UserAccount>.Success(newUserAccount);
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
