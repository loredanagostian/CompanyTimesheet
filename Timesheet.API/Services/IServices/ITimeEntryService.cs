using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface ITimeEntryService
    {
        TimeEntry? CreateTimeEntry(CreateTimeEntryDto timeEntryDto);
        List<TimeEntry> GetTimeEntries();
        List<TimeEntry> GetTimeEntriesByEmployeeIdNumber(int employeeIdNumber);
    }
}