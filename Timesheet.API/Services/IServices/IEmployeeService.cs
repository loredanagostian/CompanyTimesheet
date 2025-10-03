using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ServiceResult<IEnumerable<Employee>>> GetEmployees();
        Task<ServiceResult<Employee>> CreateEmployee(CreateEmployeeDto createEmployeeDto);
        Task<ServiceResult<Employee>> DeleteEmployee(int id);
        Task<Employee?> GetEmployeeByCNP(string cnp);
        Task<ServiceResult<Employee>> GetEmployeeById(int id);
    }
}