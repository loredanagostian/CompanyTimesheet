using Timesheet.API.Models;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployeesMockData();
        Employee CreateEmployee(Models.DTOs.CreateEmployeeDto createEmployeeDto);
        Employee? FindByEmployeeIdNumber(int employeeIdNumber);
        void RemoveEmployee(Employee employee);
        List<Employee> GetEmployees();
        void UpdateEmployeeUserAccounts(UserAccount userAccount);
    }
}
