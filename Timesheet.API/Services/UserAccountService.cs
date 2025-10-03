using System.Text.RegularExpressions;
using Timesheet.API.Extensions;
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

        private string ComputeEmail(Employee employee)
        {
            var firstNamePart = employee.FirstName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
            var lastNamePart = employee.LastName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();

            return $"{firstNamePart}.{lastNamePart}@company.com";
        }

        private Dictionary<string, string[]> Validate(CreateUserAccountDto dto)
        {
            Regex EmailRegex =
                new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var errors = new Dictionary<string, string[]>();

            var email = dto.Email.TrimToNull();
            var password = dto.Password?.Trim(); // allow spaces inside passwords but trim ends

            // Email: optional, but if present must match regex
            if (email is not null && !EmailRegex.IsMatch(email))
            {
                errors["email"] = ["Email is not a valid email address."];
            }

            // Password: optional, but if present must be >= 8 chars
            if (password is not null && password.Length < 8)
            {
                errors["password"] = ["Password must be at least 8 characters long."];
            }

            return errors;
        }

        public async Task<ServiceResult<UserAccount>> CreateUserAccount(CreateUserAccountDto userAccountDto)
        {
            var result = await _employeeService.GetEmployeeById(userAccountDto.EmployeeId);

            if (!result.IsSuccess || result.Data is null)
                return ServiceResult<UserAccount>.Failure(
                    $"No Employee was found with ID {userAccountDto.EmployeeId}."
                );

            var errors = Validate(userAccountDto);

            if (errors.Count > 0)
                return ServiceResult<UserAccount>.ValidationFailure(errors);

            var employee = result.Data;

            var newUserAccount = new UserAccount
            {
                EmployeeId = userAccountDto.EmployeeId,
                Email = userAccountDto.Email ?? ComputeEmail(employee),
                Password = userAccountDto.Password ?? "P4$$W0Rd", // Temporary password if not provided
                HasDefaultPassword = userAccountDto.Password is null,
                IsAlias = userAccountDto.Email is not null
            };

            if ((employee.UserAccounts ?? []).Any(ua => ua.Email.Equals(newUserAccount.Email)))
                return ServiceResult<UserAccount>.Failure(
                    $"An User Account already exists for Employee with ID {userAccountDto.EmployeeId} and Email {newUserAccount.Email}."
                );

            await _userAccountRepository.CreateUserAccount(newUserAccount);

            return ServiceResult<UserAccount>.Success(newUserAccount);
        }

        public async Task<ServiceResult<IEnumerable<UserAccount>>> GetUserAccounts()
        {
            return await _userAccountRepository.GetUserAccounts();
        }

        public async Task<ServiceResult<UserAccount>> GetUserAccountById(int userAccountId)
        {
            var userAccount = await _userAccountRepository.GetUserAccountById(userAccountId);

            if (userAccount is null)
                return ServiceResult<UserAccount>.Failure(
                    $"No User Account was found with ID {userAccountId}."
                );

            return ServiceResult<UserAccount>.Success(userAccount);
        }

        public async Task<ServiceResult<UserAccount>> DeleteUserAccount(int userAccountId)
        {
            var userAccount = await _userAccountRepository.GetUserAccountById(userAccountId);

            if (userAccount is null)
                return ServiceResult<UserAccount>.Failure(
                    $"No User Account was found with ID {userAccountId}."
                );

            await _userAccountRepository.DeleteUserAccount(userAccount);

            return ServiceResult<UserAccount>.Success(userAccount);
        }

        //public async Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId)
        //{
        //    return await _userAccountRepository.GetUserAccountsByEmployeeId(employeeId);
        //}
    }
}
