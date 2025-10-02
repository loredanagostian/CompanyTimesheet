using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountsController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));
        }

        [HttpPost]
        public async Task<ActionResult<UserAccountModel>> CreateUserAsync([FromBody] CreateUserAccountDto userAccountDto)
        {
            var newUserAccount = await _userAccountService.CreateUserAccountAsync(userAccountDto);

            if (newUserAccount == null)
                return NotFound("No Employee was found with this ID or Email is already in use.");

            return Ok(newUserAccount);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccountModel>>> GetUserAccountsAsync()
        {
            var userAccounts = await _userAccountService.GetUserAccountsAsync();
            return Ok(userAccounts);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUserAsync(int employeeId)
        {
            var userAccountDeletedStatus = await _userAccountService.DeleteUserAccountAsync(employeeId);

            if (userAccountDeletedStatus == 0)
                return NotFound("No User Account was found with this ID.");

            return NoContent();
        }
    }
}
