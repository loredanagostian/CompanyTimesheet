using Timesheet.API.Models;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface ITimeEntryRepository
    {
        TimeEntry? CreateTimeEntry(Models.DTOs.CreateTimeEntryDto timeEntryDto);
        List<TimeEntry> GetTimeEntries();
        List<TimeEntry> GetTimeEntriesByEmployeeIdNumber(int employeeIdNumber);
    }
}
