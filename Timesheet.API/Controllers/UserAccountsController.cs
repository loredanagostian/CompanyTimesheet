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

        //[HttpGet]
        //private ActionResult<List<UserAccountModel>> GetUserAccounts()
        //{
        //    return Ok(_userAccountService.GetUserAccounts());
        //}

        //[HttpPost]
        //private ActionResult<UserAccountModel> CreateUser([FromBody] CreateUserAccountDto userAccountDto)
        //{
        //    var newUserAccount = _userAccountService.CreateUserAccount(userAccountDto);

        //    if (newUserAccount == null)
        //        return NotFound("No Employee was found with this ID.");

        //    return Ok(newUserAccount);
        //}

        //[HttpPost("mockdata")]
        //private ActionResult<List<UserAccountModel>> CreateUserAccountsList()
        //{
        //    return Ok(_userAccountService.GetUserAccountMockData());
        //}

        //[HttpDelete]
        //private ActionResult<UserAccountModel> DeleteUser(int employeeId) 
        //{
        //    var wasUserAccountDeleted = _userAccountService.DeleteUserAccount(employeeId);

        //    if (!wasUserAccountDeleted)
        //        return NotFound("No Employee was found with this ID.");

        //    return NoContent();
        //}

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
