using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Timesheet.API.Models;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet")]
    [ApiController]
    public class CompanyTimesheetController : ControllerBase
    {
        private readonly ICompanyTimesheetService _companyTimesheetService;

        public CompanyTimesheetController(ICompanyTimesheetService companyTimesheetService)
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
