using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface ITimeEntryRepository
    {
        Task<ServiceResult<IEnumerable<TimeEntry>>> GetTimeEntries();
        Task CreateTimeEntry(TimeEntry timeEntry);
        Task<TimeEntry?> GetTimeEntryById(int id);
        Task DeleteTimeEntry(TimeEntry timeEntry);
    }
}
