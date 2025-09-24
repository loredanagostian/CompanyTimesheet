using Timesheet.API.Models;

namespace Timesheet.API.Services.Interfaces
{
    public interface ICompanyTimesheetService
    {
        CompanyTimesheet? GetTimesheetForUser(int employeeIdNumber);
    }
}