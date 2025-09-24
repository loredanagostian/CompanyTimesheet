using Timesheet.API.Models;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class CompanyTimesheetService : ICompanyTimesheetService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserAccountService _userAccountService;
        private readonly ITimeEntryService _timeEntryService;

        public CompanyTimesheetService(IEmployeeService employeeService, IUserAccountService userAccountService, ITimeEntryService timeEntryService)
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
