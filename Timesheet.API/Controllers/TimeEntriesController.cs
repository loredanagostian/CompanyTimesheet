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
        private readonly IEmployeeService _employeeService;
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntriesController(IEmployeeService employeeService, ITimeEntryService timeEntryService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _timeEntryService = timeEntryService ?? throw new ArgumentNullException(nameof(timeEntryService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntriesAsync()
        {
            return Ok(await _timeEntryService.GetTimeEntriesAsync());
        }

        [HttpPost]
        public async Task<ActionResult<TimeEntry>> CreateTimeEntryAsync([FromBody] CreateTimeEntryDto timeEntryDto)
        {
            var newTimeEntry = await _timeEntryService.CreateTimeEntryAsync(timeEntryDto);

            if (newTimeEntry == null)
                return NotFound("No Employee was found with this ID.");

            return Ok(newTimeEntry);
        }

        [HttpGet("employee/{id}")]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntriesByEmployeeIdAsync(int id)
        {
            var employeeFound = await _employeeService.GetEmployeeByIdAsync(id);

            if (employeeFound == null)
                return NotFound("No User Account was found with this Employee ID.");

            var timeEntries = await _timeEntryService.GetTimeEntriesByEmployeeIdAsync(id);

            if (timeEntries == null)
                return NotFound("No Time Entries were found for this Employee ID.");

            return Ok(timeEntries);
        }
    }
}
