using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IUserAccountRepository userAccountRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeesAsync();
        }

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            return await _employeeRepository.CreateEmployeeAsync(createEmployeeDto);
        }

        public Task<Employee?> GetEmployeeByIdAsync(int id)
            => _employeeRepository.GetEmployeeByIdAsync(id);

        public async Task RemoveEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.FindEmployeeByIdAsync(id);
            if (employee is null)
                throw new KeyNotFoundException($"No Employee was found with ID {id}.");

            await _employeeRepository.RemoveEmployeeAsync(employee);

            await _userAccountRepository.DeleteUserAccountsByEmployeeIdAsync(id);
        }

        public async Task UpdateEmployeeUserAccountsAsync(UserAccount userAccount)
        {
            await _employeeRepository.UpdateEmployeeUserAccountsAsync(userAccount);
        }

        public async Task<Employee?> FindEmployeeByCNP(string cnp)
        {
            return await _employeeRepository.FindEmployeeByCNP(cnp);
        }
    }
}
