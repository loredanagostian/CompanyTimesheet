using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<Employee?> GetEmployeeByIdAsync(int employeeIdNumber);
        Task RemoveEmployeeAsync(int employeeIdNumber);
        Task UpdateEmployeeUserAccountsAsync(UserAccount userAccount);
    }
}