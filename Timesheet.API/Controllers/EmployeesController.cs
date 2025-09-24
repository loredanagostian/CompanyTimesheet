using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly UserAccountService _userAccountService;

        public EmployeesController(EmployeeService employeeService, UserAccountService userAccountService) {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));
        }

        [HttpGet]
        public ActionResult<List<Employee>> GetEmployees()
        {
            return Ok(_employeeService.GetEmployees());
        }

        [HttpPost]
        public ActionResult<Employee> CreateEmployee([FromBody] CreateEmployeeDto employeeDto)
        {
            var newEmployee = _employeeService.CreateEmployee(employeeDto);

            return CreatedAtAction(nameof(CreateEmployee), new { id = newEmployee.EmployeeIdNumber }, newEmployee);
        }

        [HttpDelete]
        public ActionResult<Employee> RemoveEmployee(int employeeIdNumber)
        {
            var wasEmployeeRemoved = _employeeService.RemoveEmployee(employeeIdNumber);

            if (!wasEmployeeRemoved)
                return NotFound("No Employee was found with this ID.");

            var userAccountLinked = _userAccountService.FindByEmployeeIdNumber(employeeIdNumber);

            if (userAccountLinked != null && userAccountLinked != default)
            {
                var wasUserAccountRemoved = _userAccountService.DeleteUserAccount(employeeIdNumber);

                if (!wasUserAccountRemoved)
                    return NotFound("No User Account was found for this Employee ID.");
            }

            return NoContent();
        }
    }
}
