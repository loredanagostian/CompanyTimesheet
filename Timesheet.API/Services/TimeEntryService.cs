using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly IUserAccountService _userAccountService;
        private static List<TimeEntry> _timeEntries = new List<TimeEntry>();

        public TimeEntryService(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));
        }

        public TimeEntry? CreateTimeEntry(CreateTimeEntryDto timeEntryDto)
        {
            var userAccountFound = _userAccountService.FindByEmployeeIdNumber(timeEntryDto.EmployeeIdNumber);

            if (userAccountFound == null)
                return null;

            var newTimeEntry = new TimeEntry
            {
                TimeEntryId = Guid.NewGuid(),
                Date = timeEntryDto.Date,
                HoursWorked = timeEntryDto.HoursWorked,
                UserAccount = userAccountFound
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
            return _timeEntries.FindAll(t => t.UserAccount.Employee.EmployeeIdNumber == employeeIdNumber);
        }
    }
}
