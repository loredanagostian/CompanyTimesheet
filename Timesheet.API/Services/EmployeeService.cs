using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Services.Interfaces;

namespace Timesheet.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private static List<Employee> _employees = new List<Employee>();
        private static int _employeeCounter = 1;

        public List<Employee> GetEmployeesMockData()
        {
            _employees.Add(
                new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    EmployeeIdNumber = _employeeCounter++,
                    FirstName = "Ana",
                    LastName = "Blandiana",
                    ContractType = ContractType.FullTime
                }
            );

            _employees.Add(
                new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    EmployeeIdNumber = _employeeCounter++,
                    FirstName = "Ion",
                    LastName = "Gladiatorul",
                    ContractType = ContractType.PartTime
                }
            );

            _employees.Add(
                new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    EmployeeIdNumber = _employeeCounter++,
                    FirstName = "Maria",
                    LastName = "Ioana",
                    ContractType = ContractType.FullTime
                }
            );

            _employees.Add(
                new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    EmployeeIdNumber = _employeeCounter++,
                    FirstName = "Catalin",
                    LastName = "Botezatul",
                    ContractType = ContractType.Contractor
                }
            );

            return _employees;
        }

        public Employee CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                EmployeeIdNumber = _employeeCounter++,
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                ContractType = createEmployeeDto.ContractType
            };

            _employees.Add(employee);

            return employee;
        }

        public bool RemoveEmployee(int employeeIdNumber)
        {
            var employeeFound = _employees.FirstOrDefault(e => e.EmployeeIdNumber == employeeIdNumber);

            if (employeeFound == null)
                return false;

            _employees.Remove(employeeFound);

            return true;
        }

        public List<Employee> GetEmployees()
        {
            return _employees;
        }

        public Employee? FindByEmployeeIdNumber(int employeeIdNumber)
        {
            return _employees.FirstOrDefault(e => e.EmployeeIdNumber == employeeIdNumber);
        }
    }
}
