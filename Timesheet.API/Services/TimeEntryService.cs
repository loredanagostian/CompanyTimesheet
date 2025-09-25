using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;

        public TimeEntryService(IUserAccountRepository userAccountRepository, ITimeEntryRepository timeEntryRepository)
        {
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
            _timeEntryRepository = timeEntryRepository ?? throw new ArgumentNullException(nameof(timeEntryRepository));
        }

        public TimeEntry? CreateTimeEntry(CreateTimeEntryDto timeEntryDto)
        {
            var userAccountFound = _userAccountRepository.FindByEmployeeIdNumber(timeEntryDto.EmployeeId);

            if (userAccountFound == null)
                return null;

           var newTimeEntry = _timeEntryRepository.CreateTimeEntry(timeEntryDto);

            return newTimeEntry;
        }

        public List<TimeEntry> GetTimeEntries()
        {
            return _timeEntryRepository.GetTimeEntries();
        }

        public List<TimeEntry> GetTimeEntriesByEmployeeIdNumber(int employeeIdNumber)
        {
            return _timeEntryRepository.GetTimeEntriesByEmployeeIdNumber(employeeIdNumber);
        }
    }
}
