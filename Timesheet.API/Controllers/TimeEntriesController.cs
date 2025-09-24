using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntriesController(IUserAccountService userAccount, ITimeEntryService timeEntryService)
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
            var newTimeEntry = _timeEntryService.CreateTimeEntry(timeEntryDto);

            if (newTimeEntry == null)
                return NotFound("No Employee was found with this ID.");

            return Ok(newTimeEntry);
        }
    }
}
