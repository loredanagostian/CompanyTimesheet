using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static List<EmployeeModel> _employees = new List<EmployeeModel>();
        private static int _employeeCounter = 1;

        private readonly TimesheetContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(TimesheetContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<EmployeeModel> GetEmployeesMockData()
        {
            _employees.Add(
                new EmployeeModel
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Ana",
                    LastName = "Blandiana",
                    ContractType = ContractType.FullTime,
                    CNP = "1234567890123"
                }
            );

            _employees.Add(
                new EmployeeModel
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Ion",
                    LastName = "Gladiatorul",
                    ContractType = ContractType.PartTime,
                    CNP = "9876543210987"
                }
            );

            _employees.Add(
                new EmployeeModel
                {
                    EmployeeId = _employeeCounter++,
                    FirstName = "Maria",
                    LastName = "Ioana",
                    ContractType = ContractType.FullTime,
                    CNP = "4567891234567"
                }
            );

            _employees.Add(
                new EmployeeModel
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

        public EmployeeModel CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            var employee = new EmployeeModel
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

        //public Task<EmployeeModel> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        //{
        //    var employee = new EmployeeModel
        //    {
        //        EmployeeId = _employeeCounter++,
        //        FirstName = createEmployeeDto.FirstName,
        //        LastName = createEmployeeDto.LastName,
        //        ContractType = createEmployeeDto.ContractType,
        //        CNP = createEmployeeDto.CNP
        //    };

        //    _employees.Add(employee);

        //    return employee;
        //}

        public EmployeeModel? FindByEmployeeIdNumber(int employeeIdNumber)
        {
            return _employees.FirstOrDefault(e => e.EmployeeId == employeeIdNumber);
        }

        public void RemoveEmployee(EmployeeModel employee)
        {
            _employees.Remove(employee);
        }

        public List<EmployeeModel> GetEmployees()
        {
            return _employees;
        }

        public void UpdateEmployeeUserAccounts(UserAccountModel userAccount)
        {
            var employee = FindByEmployeeIdNumber(userAccount.EmployeeId);

            if (employee != null)
            {
                employee.UserAccounts.Add(userAccount);
            }
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();

            // Without AutoMapper
            //
            //return employees.Select(e => new EmployeeModel
            //{
            //    EmployeeId = e.EmployeeId,
            //    FirstName = e.FirstName,
            //    LastName = e.LastName,
            //    ContractType = e.ContractType,
            //    CNP = e.CNP
            //}).ToList();

            return _mapper.Map<IEnumerable<EmployeeModel>>(employees);
        }
    }
}
