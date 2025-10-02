using Timesheet.API.Entities;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntryModel>> GetTimeEntriesAsync();
        Task<(TimeEntryModel, TimeEntry)> CreateTimeEntryAsync(CreateTimeEntryDto timeEntryDto);
        Task<IEnumerable<TimeEntryModel>> GetTimeEntriesByEmployeeIdAsync(int id);
    }
}
