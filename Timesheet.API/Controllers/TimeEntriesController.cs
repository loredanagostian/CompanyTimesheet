using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly UserAccountService _userAccountService;
        private readonly TimeEntryService _timeEntryService;

        public TimeEntriesController(UserAccountService userAccount, TimeEntryService timeEntryService)
        {
            _userAccountService = userAccount ?? throw new ArgumentNullException(nameof(userAccount));
            _timeEntryService = timeEntryService ?? throw new ArgumentNullException(nameof(timeEntryService));
        }

        [HttpGet]
        public ActionResult<List<TimeEntry>> GetTimeEntries()
        {
            return Ok(_timeEntryService.GetTimeEntries());
        }

        [HttpPost]
        public ActionResult<TimeEntry> CreateTimeEntry([FromBody] CreateTimeEntryDto timeEntryDto)
        {
            var userAccountFound = _userAccountService.FindByEmployeeIdNumber(timeEntryDto.EmployeeIdNumber);

            if (userAccountFound == null)
                return NotFound("No UserAccount was found with this Email.");

            var newTimeEntry = _timeEntryService.CreateTimeEntry(timeEntryDto);

            return Ok(newTimeEntry);
        }
    }
}
