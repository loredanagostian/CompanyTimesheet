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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUserAccounts()
        {
            var result = await _userAccountService.GetUserAccounts();

            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserAccount>> CreateUser([FromBody] CreateUserAccountDto userAccountDto)
        {
            var result = await _userAccountService.CreateUserAccount(userAccountDto);

            if (result.IsSuccess && result.Data is null)
                return Problem("An unexpected error occurred while creating the User Account.");

            return result.IsSuccess
                ? CreatedAtRoute("GetUserAccountById", new { id = result.Data!.UserAccountId }, result.Data)
                : result.HasValidationErrors 
                    ? BadRequest(result.ValidationErrors)
                    : BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}", Name = "GetUserAccountById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserAccount>> GetUserAccountById(int id)
        {
            var result = await _userAccountService.GetUserAccountById(id);

            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteUserAccount(int userAccountId)
        {
            var result = await _userAccountService.DeleteUserAccount(userAccountId);

            return result.IsSuccess
                ? NoContent()
                : BadRequest(result.ErrorMessage);
        }
    }
}
