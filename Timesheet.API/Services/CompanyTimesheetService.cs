using Timesheet.API.Models;

namespace Timesheet.API.Services
{
    public class CompanyTimesheetService
    {
        private readonly EmployeeService _employeeService;
        private readonly UserAccountService _userAccountService;
        private readonly TimeEntryService _timeEntryService;

        public CompanyTimesheetService(EmployeeService employeeService, UserAccountService userAccountService, TimeEntryService timeEntryService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));
            _timeEntryService = timeEntryService ?? throw new ArgumentNullException(nameof(_timeEntryService));
        }

        public CompanyTimesheet? GetTimesheetForUser(int employeeIdNumber)
        {
            var userFound = _userAccountService.FindByEmployeeIdNumber(employeeIdNumber);

            if (userFound == null) return null;

            var employeeFound = _employeeService.FindByEmployeeIdNumber(employeeIdNumber);

            if (employeeFound == null) return null;

            var timeEntries = _timeEntryService.GetTimeEntriesByEmployeeIdNumber(employeeIdNumber);

            return new CompanyTimesheet
            {
                EmployeeIdNumber = employeeIdNumber,
                FullName = $"{employeeFound.FirstName} {employeeFound.LastName}",
                Email = userFound.Email,
                ContractType = employeeFound.ContractType,
                TimeEntries = timeEntries
            };
        }
    }
}
