using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface ITimeEntryService
    {
        TimeEntryModel? CreateTimeEntry(CreateTimeEntryDto timeEntryDto);
        List<TimeEntryModel> GetTimeEntries();
        List<TimeEntryModel> GetTimeEntriesByEmployeeIdNumber(int employeeIdNumber);
    }
}