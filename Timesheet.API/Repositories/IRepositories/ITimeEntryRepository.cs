using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntry>> GetTimeEntriesAsync();
        Task<(TimeEntry, TimeEntry)> CreateTimeEntryAsync(CreateTimeEntryDto timeEntryDto);
        Task<IEnumerable<TimeEntry>> GetTimeEntriesByEmployeeIdAsync(int id);
    }
}
