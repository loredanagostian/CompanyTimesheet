//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Extensions;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly TimesheetContext _context;
        //private readonly IMapper _mapper;

        public EmployeeRepository(TimesheetContext context /*, IMapper mapper*/)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.UserAccounts)
                .AsNoTracking()
                .ToListAsync();

            return employees;
        }

        public async Task<Employee> CreateEmployee(CreateEmployeeDto employeeDto)
        {
            var contractTypeParsed = Enum.Parse<ContractType>(employeeDto.ContractType!.Trim(), ignoreCase: true);

            //var employeeEntity = _mapper.Map<Employee>(employeeDto);
            var newEmployee = new Employee
            {
                FirstName = employeeDto.FirstName.CapitalizeFirstLetter(),
                LastName = employeeDto.LastName.CapitalizeFirstLetter(),
                CNP = employeeDto.CNP,
                ContractType = contractTypeParsed,
            };

            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();

            return newEmployee;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            var entity = await FindEmployeeByIdAsync(id);
            return entity is null ? null : entity;
        }

        public async Task<Employee?> FindEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task AddEmployeeUserAccount(Employee employee, UserAccount userAccount)
        {
            employee.UserAccounts.Add(userAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeTimeEntriesAsync(TimeEntry timeEntry)
        {
            var employee = await FindEmployeeByIdAsync(timeEntry.EmployeeId);

            if (employee != null)
            {
                if (employee.TimeEntries == null)
                    employee.TimeEntries = new List<TimeEntry>();

                if (!employee.TimeEntries.Contains(timeEntry))
                    employee.TimeEntries.Add(timeEntry);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Employee?> FindEmployeeByCNP(string cnp)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CNP == cnp);

            return employee;
        }
    }
}
