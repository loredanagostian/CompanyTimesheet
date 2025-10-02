using Timesheet.API.Entities;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeModel>> GetEmployeesAsync();
        Task<EmployeeModel> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeIdNumber);
        Task RemoveEmployeeAsync(Employee employee);
        Task<Employee?> FindEmployeeByIdAsync(int id);
        Task UpdateEmployeeUserAccountsAsync(UserAccount userAccount);
        Task UpdateEmployeeTimeEntriesAsync(TimeEntry timeEntry);
    }
}
