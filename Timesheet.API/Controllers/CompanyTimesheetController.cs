using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Timesheet.API.Models;
using Timesheet.API.Services;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet")]
    [ApiController]
    public class CompanyTimesheetController : ControllerBase
    {
        private readonly CompanyTimesheetService _companyTimesheetService;

        public CompanyTimesheetController(CompanyTimesheetService companyTimesheetService)
        {
            _companyTimesheetService = companyTimesheetService ?? throw new ArgumentNullException(nameof(companyTimesheetService));
        }

        [HttpGet]
        public ActionResult<CompanyTimesheet> GetTimesheetForUser([Required] int employeeIdNumber)
        {
            var timesheetEntriesForEmployee = _companyTimesheetService.GetTimesheetForUser(employeeIdNumber);

            if (timesheetEntriesForEmployee == null) 
                return NotFound();

            return Ok(timesheetEntriesForEmployee);
        }
    }
}
