using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDto createEmployeeDto);
        Employee? FindByEmployeeIdNumber(int employeeIdNumber);
        List<Employee> GetEmployees();
        List<Employee> GetEmployeesMockData();
        bool RemoveEmployee(int employeeIdNumber);
        void UpdateEmployeeUserAccounts(UserAccount userAccount);
    }
}