using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        EmployeeModel CreateEmployee(CreateEmployeeDto createEmployeeDto);
        EmployeeModel? FindByEmployeeIdNumber(int employeeIdNumber);
        List<EmployeeModel> GetEmployees();
        List<EmployeeModel> GetEmployeesMockData();
        bool RemoveEmployee(int employeeIdNumber);
        void UpdateEmployeeUserAccounts(UserAccountModel userAccount);
        Task<IEnumerable<EmployeeModel>> GetEmployeesAsync();
        Task<EmployeeModel> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeIdNumber);
    }
}