using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;
using Timesheet.API.Validations;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IEmployeeService _employeeService;

        public UserAccountsController(IUserAccountService userAccountService, IEmployeeService employeeService)
        {
            _userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpPost]
        public async Task<ActionResult<UserAccount>> CreateUser([FromBody] CreateUserAccountDto userAccountDto)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(userAccountDto.EmployeeId);

            if (employee == null)
                return NotFound($"No Employee was found with ID {userAccountDto.EmployeeId}.");

            var errors = UserAccountValidation.Validate(userAccountDto);
            if (errors.Count > 0)
            {
                return ValidationProblem(new ValidationProblemDetails(errors)
                {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var newUserAccount = await _userAccountService.CreateUserAccount(userAccountDto, employee);

            if (newUserAccount == null)
                return Conflict($"Email {userAccountDto.Email} is already in use.");

            return CreatedAtAction(nameof(GetUserAccountById), new { id = newUserAccount.UserAccountId }, newUserAccount);
        }

        [HttpGet("{id}", Name = "GetUserAccountById")]
        public async Task<ActionResult<UserAccount>> GetUserAccountById(int id)
        {
            var userAccount = await _userAccountService.GetUserAccountsAsync();
            if (userAccount == null)
                return NotFound("No User Account was found with this ID.");
            return Ok(userAccount);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUserAccounts()
        {
            var userAccounts = await _userAccountService.GetUserAccountsAsync();
            return Ok(userAccounts);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int employeeId)
        {
            var userAccountDeletedStatus = await _userAccountService.DeleteUserAccountAsync(employeeId);

            if (userAccountDeletedStatus == 0)
                return NotFound("No User Account was found with this ID.");

            return NoContent();
        }
    }
}
