using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<Employee?> GetEmployeeByIdAsync(int employeeIdNumber);
        Task RemoveEmployeeAsync(Employee employee);
        Task<Employee?> FindEmployeeByIdAsync(int id);
        Task UpdateEmployeeUserAccountsAsync(UserAccount userAccount);
        Task UpdateEmployeeTimeEntriesAsync(TimeEntry timeEntry);
        Task<Employee?> FindEmployeeByCNP(string cnp);
    }
}
