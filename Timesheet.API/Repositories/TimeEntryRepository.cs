using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        //private static List<TimeEntryModel> _timeEntries = new List<TimeEntryModel>();

        private TimeEntryModel? CreateTimeEntry(CreateTimeEntryDto timeEntryDto)
        {
            var newTimeEntry = new TimeEntryModel
            {
                TimeEntryId = Guid.NewGuid(),
                Date = timeEntryDto.Date,
                HoursWorked = timeEntryDto.HoursWorked,
                EmployeeId = timeEntryDto.EmployeeId,
            };

            _timeEntries.Add(newTimeEntry);

            return newTimeEntry;
        }

        private List<TimeEntryModel> GetTimeEntries()
        {
            return _timeEntries;
        }

        private List<TimeEntryModel> GetTimeEntriesByEmployeeIdNumber(int employeeIdNumber)
        {
            return _timeEntries.FindAll(t => t.EmployeeId == employeeIdNumber);
        }
    }
}
