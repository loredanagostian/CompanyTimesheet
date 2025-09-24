using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services
{
    public class TimeEntryService
    {
        private static List<TimeEntry> _timeEntries = new List<TimeEntry>();

        public TimeEntry CreateTimeEntry(CreateTimeEntryDto timeEntryDto)
        {
            var newTimeEntry = new TimeEntry
            {
                TimeEntryId = Guid.NewGuid(),
                EmployeeIdNumber = timeEntryDto.EmployeeIdNumber,
                Date = timeEntryDto.Date,
                HoursWorked = timeEntryDto.HoursWorked
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
            return _timeEntries.FindAll(t => t.EmployeeIdNumber == employeeIdNumber);
        }
    }
}
