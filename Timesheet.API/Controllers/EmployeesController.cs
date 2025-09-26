using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Controllers
{
    [Route("api/timesheet/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService employeeService, IUserAccountService userAccountService, IMapper mapper) {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        private ActionResult<List<EmployeeModel>> GetEmployees()
        {
            return Ok(_employeeService.GetEmployees());
        }

        [HttpPost]
        private ActionResult<EmployeeModel> CreateEmployee([FromBody] CreateEmployeeDto employeeDto)
        {
            var newEmployee = _employeeService.CreateEmployee(employeeDto);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
        }

        [HttpDelete]
        private ActionResult<EmployeeModel> RemoveEmployee(int employeeIdNumber)
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

        [HttpGet("async")]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployeesAsync()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            
            return Ok(employees);
        }

        [HttpPost("async")]
        public async Task<ActionResult<EmployeeModel>> CreateEmployeeAsync([FromBody] CreateEmployeeDto employeeDto)
        {
            var newEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);
            
            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public async Task<ActionResult<EmployeeModel>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee is null) return NotFound();
            
            return Ok(employee);
        }

        [HttpDelete("async")]
        public async Task<ActionResult> RemoveEmployeeAsync(int employeeIdNumber)
        {
            await _employeeService.RemoveEmployeeAsync(employeeIdNumber);

            return NoContent();
        }
    }
}
