using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> CreateEmployee(CreateEmployeeDto createEmployeeDto);
        Task<Employee?> GetEmployeeByIdAsync(int employeeIdNumber);
        Task DeleteEmployee(Employee employee);
        Task<Employee?> FindEmployeeByIdAsync(int id);
        Task AddEmployeeUserAccount(Employee employee, UserAccount userAccount);
        Task UpdateEmployeeTimeEntriesAsync(TimeEntry timeEntry);
        Task<Employee?> FindEmployeeByCNP(string cnp);
    }
}
