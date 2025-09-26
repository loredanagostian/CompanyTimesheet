using Timesheet.API.Entities;
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

        public EmployeeModel CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var newEmployee = _employeeRepository.CreateEmployee(createEmployeeDto);

            return newEmployee;
        }

        public bool RemoveEmployee(int employeeIdNumber)
        {
            var employeeFound = _employeeRepository.FindByEmployeeIdNumber(employeeIdNumber);

            if (employeeFound == null)
                return false;

            if (employeeFound.UserAccounts.Any())
            {
                foreach (var userAccount in employeeFound.UserAccounts)
                {
                    _userAccountRepository.DeleteUserAccount(userAccount);
                }
            }

            _employeeRepository.RemoveEmployee(employeeFound);

            return true;
        }

        public List<EmployeeModel> GetEmployees()
        {
            return _employeeRepository.GetEmployees();
        }

        public EmployeeModel? FindByEmployeeIdNumber(int employeeIdNumber)
        {
            return _employeeRepository.FindByEmployeeIdNumber(employeeIdNumber);
        }

        public void UpdateEmployeeUserAccounts(UserAccountModel userAccount)
        {
            _employeeRepository.UpdateEmployeeUserAccounts(userAccount);
        }

        public List<EmployeeModel> GetEmployeesMockData()
        {
            return _employeeRepository.GetEmployeesMockData();
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeesAsync();
        }

        public async Task<EmployeeModel> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            return await _employeeRepository.CreateEmployeeAsync(createEmployeeDto);
        }

        public Task<EmployeeModel?> GetEmployeeByIdAsync(int id)
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
    }
}
