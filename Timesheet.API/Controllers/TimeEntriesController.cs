using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntriesController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService ?? throw new ArgumentNullException(nameof(timeEntryService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntries()
        {
            var result = await _timeEntryService.GetTimeEntries();

            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TimeEntry>> CreateTimeEntry([FromBody] CreateTimeEntryDto timeEntryDto)
        {
            var result = await _timeEntryService.CreateTimeEntry(timeEntryDto);

            if (result.IsSuccess && result.Data is null)
                return Problem("An unexpected error occurred while creating the User Account.");

            return result.IsSuccess
                ? CreatedAtRoute("GetTimeEntryById", new { id = result.Data!.TimeEntryId }, result.Data)
                : result.HasValidationErrors
                    ? BadRequest(result.ValidationErrors)
                    : BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}", Name = "GetTimeEntryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimeEntry>> GetTimeEntryById(int id)
        {
            var result = await _timeEntryService.GetTimeEntryById(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var result = await _timeEntryService.DeleteTimeEntry(id);

            return result.IsSuccess
                ? NoContent()
                : BadRequest(result.ErrorMessage);
        }
    }
}
