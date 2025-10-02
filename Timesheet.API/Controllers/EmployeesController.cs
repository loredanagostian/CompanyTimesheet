using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Models.Validations;
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
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesAsync()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            
            return Ok(employees);
        }

        [HttpPost]
        //[ApiVersion("3.0")]
        public async Task<ActionResult<Employee>> CreateEmployeeAsync([FromBody] CreateEmployeeDto employeeDto)
        {
            var errors = EmployeeValidation.Validate(employeeDto);
            if (errors.Count > 0)
            {
                return ValidationProblem(new ValidationProblemDetails(errors)
                {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var employeeExists = await _employeeService.FindEmployeeByCNP(employeeDto.CNP);

            if (employeeExists is not null)
            {
                return Conflict($"An employee with CNP {employeeDto.CNP} already exists.");
            }

            var newEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);
            
            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee is null) return NotFound();
            
            return Ok(employee);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveEmployeeAsync(int employeeIdNumber)
        {
            await _employeeService.RemoveEmployeeAsync(employeeIdNumber);

            return NoContent();
        }
    }
}
