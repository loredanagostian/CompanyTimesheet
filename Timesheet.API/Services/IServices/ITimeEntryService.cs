using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface ITimeEntryService
    {
        Task<ServiceResult<IEnumerable<TimeEntry>>> GetTimeEntries();
        Task<ServiceResult<TimeEntry>> CreateTimeEntry(CreateTimeEntryDto timeEntryDto);
        Task<ServiceResult<TimeEntry>> GetTimeEntryById(int timeEntryId);
        Task<ServiceResult<TimeEntry>> DeleteTimeEntry(int timeEntryId);
    }
}