using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task CreateEmployee(Employee employee);
        Task<Employee?> GetEmployeeById(int id);
        Task DeleteEmployee(Employee employee);
        Task<Employee?> GetEmployeeByCNP(string cnp);
    }
}
