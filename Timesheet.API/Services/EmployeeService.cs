using System.Text.RegularExpressions;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private Regex NameRegex = new(@"^[\p{L}\p{M}'\- ]+$", RegexOptions.Compiled);
        private Regex CnpRegex = new(@"^\d{13}$", RegexOptions.Compiled);
        private HashSet<string> AllowedContractTypes =
            new(StringComparer.OrdinalIgnoreCase) { "FullTime", "PartTime", "Contractor" };

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        private Dictionary<string, string[]> Validate(CreateEmployeeDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            string? first = dto.FirstName.Trim();
            string? last = dto.LastName.Trim();
            string? cnp = dto.CNP.Trim();
            string? type = dto.ContractType.Trim();

            if (string.IsNullOrWhiteSpace(first) || !NameRegex.IsMatch(first))
                errors["firstName"] = ["First name may contain letters, spaces, apostrophes, and hyphens only."];

            if (string.IsNullOrWhiteSpace(last) || !NameRegex.IsMatch(last))
                errors["lastName"] = ["Last name may contain letters, spaces, apostrophes, and hyphens only."];

            if (string.IsNullOrWhiteSpace(cnp) || !CnpRegex.IsMatch(cnp))
                errors["cnp"] = ["CNP must be exactly 13 digits."];

            if (string.IsNullOrWhiteSpace(type) || !AllowedContractTypes.Contains(type))
                errors["contractType"] = ["ContractType must be one of: FullTime, PartTime, Contractor."];

            return errors;
        }

        private ContractType ParseContractType(string contractType)
        {
            return Enum.Parse<ContractType>(contractType.Trim(), ignoreCase: true);
        }

        public async Task<ServiceResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployees();

            return ServiceResult<IEnumerable<Employee>>.Success(employees);
        }

        public async Task<ServiceResult<Employee>> CreateEmployee(CreateEmployeeDto employeeDto)
        {
            var errors = Validate(employeeDto);

            if (errors.Count > 0)
                return ServiceResult<Employee>.ValidationFailure(errors);

            var newEmployee = new Employee
            {
                FirstName = employeeDto.FirstName.Trim(),
                LastName = employeeDto.LastName.Trim(),
                CNP = employeeDto.CNP.Trim(),
                ContractType = ParseContractType(employeeDto.ContractType),
            };

            var employeeExists = await _employeeRepository.GetEmployeeByCNP(newEmployee.CNP);

            if (employeeExists is not null)
                return ServiceResult<Employee>.Failure($"An employee with CNP {newEmployee.CNP} already exists.");

            await _employeeRepository.CreateEmployee(newEmployee);

            return ServiceResult<Employee>.Success(newEmployee);
        }

        public async Task<ServiceResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee is null)
                return ServiceResult<Employee>.Failure(
                    $"Employee with ID {id} not found."
                );

            return ServiceResult<Employee>.Success(employee);
        }

        public async Task<ServiceResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee is null)
                return ServiceResult<Employee>.Failure(
                    $"Employee with ID {id} not found."
                );

            await _employeeRepository.DeleteEmployee(employee);

            return ServiceResult<Employee>.Success(employee);
        }

        public async Task<Employee?> GetEmployeeByCNP(string cnp)
        {
            return await _employeeRepository.GetEmployeeByCNP(cnp);
        }
    }
}
