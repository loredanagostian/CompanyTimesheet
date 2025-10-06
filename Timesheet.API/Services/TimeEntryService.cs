using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITimeEntryRepository _timeEntryRepository;

        public TimeEntryService(IEmployeeService employeeService, ITimeEntryRepository timeEntryRepository)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _timeEntryRepository = timeEntryRepository ?? throw new ArgumentNullException(nameof(timeEntryRepository));
        }

        private Dictionary<string, string[]> Validate(CreateTimeEntryDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            // 1) HoursWorked must be between 0 and 8 inclusive
            if (dto.HoursWorked < 0 || dto.HoursWorked > 8)
                errors["hoursWorked"] = ["HoursWorked must be between 0 and 8 hours."];

            // 2) Date sanity: guard against default
            if (dto.Date == default)
                errors["date"] = ["Date is required and must be a valid date/time."];

            // Use DateOnly for all date rules
            var selected = DateOnly.FromDateTime(dto.Date);

            // "Now" anchor (UTC or your local TZ — choose one and stick to it)
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // 3) Date cannot be in the next calendar month (relative to 'today')
            var (nextYear, nextMonth) = NextMonthOf(today.Year, today.Month);
            if (selected.Year == nextYear && selected.Month == nextMonth)
                errors["date"] = ["Date cannot be in the next calendar month."];

            // 4) Forbid any date after the current month’s last day
            var lastDayThisMonth = new DateOnly(today.Year, today.Month,
                DateTime.DaysInMonth(today.Year, today.Month));
            if (selected > lastDayThisMonth)
                errors["date"] = ["Date must be within the current month or earlier."];

            return errors;
        }

        private static (int year, int month) NextMonthOf(int year, int month)
            => month == 12 ? (year + 1, 1) : (year, month + 1);

        public async Task<ServiceResult<IEnumerable<TimeEntry>>> GetTimeEntries()
        {
            return await _timeEntryRepository.GetTimeEntries();
        }

        public async Task<ServiceResult<TimeEntry>> CreateTimeEntry(CreateTimeEntryDto timeEntryDto)
        {
            var result = await _employeeService.GetEmployeeById(timeEntryDto.EmployeeId);

            if (!result.IsSuccess || result.Data is null)
                return ServiceResult<TimeEntry>.Failure(
                    $"No Employee was found with ID {timeEntryDto.EmployeeId}."
                );

            var errors = Validate(timeEntryDto);

            if (errors.Count > 0)
                return ServiceResult<TimeEntry>.ValidationFailure(errors);

            var employee = result.Data;

            // Normalize to DateOnly (drop any time-of-day)
            var selectedDate = DateOnly.FromDateTime(timeEntryDto.Date);

            if ((employee.TimeEntries ?? []).Any(te => te.Date.Equals(selectedDate)))
                return ServiceResult<TimeEntry>.Failure(
                    $"A Time Entry already exists for Employee with ID {timeEntryDto.EmployeeId} and for Date {selectedDate}."
                );

            var newTimeEntry = new TimeEntry
            {
                EmployeeId = timeEntryDto.EmployeeId,
                Date = selectedDate,
                HoursWorked = timeEntryDto.HoursWorked
            };
       
            await _timeEntryRepository.CreateTimeEntry(newTimeEntry);

            return ServiceResult<TimeEntry>.Success(newTimeEntry);
        }

        public async Task<ServiceResult<TimeEntry>> GetTimeEntryById(int timeEntryId)
        {
            var timeEntry = await _timeEntryRepository.GetTimeEntryById(timeEntryId);

            if (timeEntry is null)
                return ServiceResult<TimeEntry>.Failure(
                    $"No User Account was found with ID {timeEntry}."
                );

            return ServiceResult<TimeEntry>.Success(timeEntry);
        }

        public async Task<ServiceResult<TimeEntry>> DeleteTimeEntry(int timeEntryId)
        {
            var timeEntry = await _timeEntryRepository.GetTimeEntryById(timeEntryId);

            if (timeEntry is null)
                return ServiceResult<TimeEntry>.Failure(
                    $"No Time Entry was found with ID {timeEntryId}."
                );

            await _timeEntryRepository.DeleteTimeEntry(timeEntry);

            return ServiceResult<TimeEntry>.Success(timeEntry);
        }
    }
}
