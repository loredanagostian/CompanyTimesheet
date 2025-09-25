using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private static List<TimeEntry> _timeEntries = new List<TimeEntry>();

        public TimeEntry? CreateTimeEntry(CreateTimeEntryDto timeEntryDto)
        {
            var newTimeEntry = new TimeEntry
            {
                TimeEntryId = Guid.NewGuid(),
                Date = timeEntryDto.Date,
                HoursWorked = timeEntryDto.HoursWorked,
                EmployeeId = timeEntryDto.EmployeeId,
            };

            _timeEntries.Add(newTimeEntry);

            return newTimeEntry;
        }

        public List<TimeEntry> GetTimeEntries()
        {
            return _timeEntries;
        }

        public List<TimeEntry> GetTimeEntriesByEmployeeIdNumber(int employeeIdNumber)
        {
            return _timeEntries.FindAll(t => t.EmployeeId == employeeIdNumber);
        }
    }
}
