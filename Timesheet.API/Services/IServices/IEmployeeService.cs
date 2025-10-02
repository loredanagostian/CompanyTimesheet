using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<Employee?> GetEmployeeByIdAsync(int employeeIdNumber);
        Task DeleteEmployee(Employee employee);
        Task UpdateEmployeeUserAccountsAsync(UserAccount userAccount);
        Task<Employee?> FindEmployeeByCNP(string cnp);
    }
}