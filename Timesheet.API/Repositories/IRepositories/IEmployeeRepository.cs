using Timesheet.API.Models;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        List<EmployeeModel> GetEmployeesMockData();
        EmployeeModel CreateEmployee(Models.DTOs.CreateEmployeeDto createEmployeeDto);
        EmployeeModel? FindByEmployeeIdNumber(int employeeIdNumber);
        void RemoveEmployee(EmployeeModel employee);
        List<EmployeeModel> GetEmployees();
        void UpdateEmployeeUserAccounts(UserAccountModel userAccount);
        Task<IEnumerable<EmployeeModel>> GetEmployeesAsync();
    }
}
