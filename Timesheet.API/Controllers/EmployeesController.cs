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

        public EmployeesController(IEmployeeService employeeService) {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var result = await _employeeService.GetEmployees();
            
            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ApiVersion("3.0")]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] CreateEmployeeDto employeeDto)
        {
            var result = await _employeeService.CreateEmployee(employeeDto);

            if (result.IsSuccess && result.Data is null)
                return Problem("An unexpected error occurred while creating the User Account.");

            return result.IsSuccess
                ? CreatedAtRoute("GetEmployeeById", new { id = result.Data!.EmployeeId }, result.Data)
                : result.HasValidationErrors
                    ? BadRequest(result.ValidationErrors)
                    : BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var result = await _employeeService.GetEmployeeById(id);
            
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteEmployee(int employeeId)
        {
            var result = await _employeeService.DeleteEmployee(employeeId);

            return result.IsSuccess
                ? NoContent()
                : BadRequest(result.ErrorMessage);
        }
    }
}
