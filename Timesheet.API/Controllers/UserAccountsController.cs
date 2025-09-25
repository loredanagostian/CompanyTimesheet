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

        [HttpGet]
        public ActionResult<List<UserAccountModel>> GetUserAccounts()
        {
            return Ok(_userAccountService.GetUserAccounts());
        }

        [HttpPost]
        public ActionResult<UserAccountModel> CreateUser([FromBody] CreateUserAccountDto userAccountDto)
        {
            var newUserAccount = _userAccountService.CreateUserAccount(userAccountDto);

            if (newUserAccount == null)
                return NotFound("No Employee was found with this ID.");

            return Ok(newUserAccount);
        }

        [HttpPost("mockdata")]
        public ActionResult<List<UserAccountModel>> CreateUserAccountsList()
        {
            return Ok(_userAccountService.GetUserAccountMockData());
        }

        [HttpDelete]
        public ActionResult<UserAccountModel> DeleteUser(int employeeId) 
        {
            var wasUserAccountDeleted = _userAccountService.DeleteUserAccount(employeeId);

            if (!wasUserAccountDeleted)
                return NotFound("No Employee was found with this ID.");

            return NoContent();
        }
    }
}
