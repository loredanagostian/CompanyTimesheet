using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        List<EmployeeModel> GetEmployeesMockData();
        EmployeeModel CreateEmployee(CreateEmployeeDto createEmployeeDto);
        EmployeeModel? FindByEmployeeIdNumber(int employeeIdNumber);
        void RemoveEmployee(EmployeeModel employee);
        List<EmployeeModel> GetEmployees();
        void UpdateEmployeeUserAccounts(UserAccountModel userAccount);
        Task<IEnumerable<EmployeeModel>> GetEmployeesAsync();
        Task<EmployeeModel> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeIdNumber);
    }
}
