using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;

        public TimeEntryService(IEmployeeRepository employeeRepository, ITimeEntryRepository timeEntryRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _timeEntryRepository = timeEntryRepository ?? throw new ArgumentNullException(nameof(timeEntryRepository));
        }

        public async Task<IEnumerable<TimeEntry>> GetTimeEntriesAsync()
        {
            return await _timeEntryRepository.GetTimeEntriesAsync();
        }

        public async Task<TimeEntry?> CreateTimeEntryAsync(CreateTimeEntryDto timeEntryDto)
        {
            var employeeFound = await _employeeRepository.GetEmployeeById(timeEntryDto.EmployeeId);

            if (employeeFound is null)
                return null;

            var newTimeEntry = await _timeEntryRepository.CreateTimeEntryAsync(timeEntryDto);

            //await _employeeRepository.UpdateEmployeeTimeEntriesAsync(newTimeEntry);

            return newTimeEntry;
        }

        public async Task<IEnumerable<TimeEntry>> GetTimeEntriesByEmployeeIdAsync(int id)
        {
            return await _timeEntryRepository.GetTimeEntriesByEmployeeIdAsync(id);
        }
    }
}
