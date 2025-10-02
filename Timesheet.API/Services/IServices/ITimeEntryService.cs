using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface ITimeEntryService
    {
        Task<IEnumerable<TimeEntryModel>> GetTimeEntriesAsync();
        Task<TimeEntryModel?> CreateTimeEntryAsync(CreateTimeEntryDto timeEntryDto);
        Task<IEnumerable<TimeEntryModel>> GetTimeEntriesByEmployeeIdAsync(int id);
    }
}