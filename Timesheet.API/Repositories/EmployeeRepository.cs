using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static List<Employee> _employees = new List<Employee>();
        private static int _employeeCounter = 1;

        public List<Employee> GetEmployeesMockData()
        {
            _employees.Add(
                new Employee
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Ana",
                    LastName = "Blandiana",
                    ContractType = ContractType.FullTime,
                    CNP = "1234567890123"
                }
            );

            _employees.Add(
                new Employee
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Ion",
                    LastName = "Gladiatorul",
                    ContractType = ContractType.PartTime,
                    CNP = "9876543210987"
                }
            );

            _employees.Add(
                new Employee
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Maria",
                    LastName = "Ioana",
                    ContractType = ContractType.FullTime,
                    CNP = "4567891234567"
                }
            );

            _employees.Add(
                new Employee
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Catalin",
                    LastName = "Botezatul",
                    ContractType = ContractType.Contractor,
                    CNP = "7891234567891"
                }
            );

            return _employees;
        }

        public Employee CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var employee = new Employee
            {
                EmployeeId = _employeeCounter++,
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                ContractType = createEmployeeDto.ContractType,
                CNP = createEmployeeDto.CNP
            };

            _employees.Add(employee);

            return employee;
        }

        public Employee? FindByEmployeeIdNumber(int employeeIdNumber)
        {
            return _employees.FirstOrDefault(e => e.EmployeeId == employeeIdNumber);
        }

        public void RemoveEmployee(Employee employee)
        {
            _employees.Remove(employee);
        }

        public List<Employee> GetEmployees()
        {
            return _employees;
        }

        public void UpdateEmployeeUserAccounts(UserAccount userAccount)
        {
            var employee = FindByEmployeeIdNumber(userAccount.EmployeeId);

            if (employee != null)
            {
                employee.UserAccounts.Add(userAccount);
            }
        }
    }
}
