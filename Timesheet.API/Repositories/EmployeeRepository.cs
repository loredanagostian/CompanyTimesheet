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
        private readonly TimesheetContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(TimesheetContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .ToListAsync();

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

            return employees;
        }

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            var employeeEntity = _mapper.Map<Employee>(employeeDto);
            await _context.Employees.AddAsync(employeeEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Employee>(employeeEntity);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            var entity = await FindEmployeeByIdAsync(id);
            return entity is null ? null : _mapper.Map<Employee>(entity);
        }

        public async Task<Employee?> FindEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task RemoveEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeUserAccountsAsync(UserAccount userAccount)
        {
            var employee = await FindEmployeeByIdAsync(userAccount.EmployeeId);

            if (employee != null)
            {
                if (employee.UserAccounts == null)
                    employee.UserAccounts = new List<UserAccount>();

                if (!employee.UserAccounts.Contains(userAccount))
                    employee.UserAccounts.Add(userAccount);

                await _context.SaveChangesAsync();
            }
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
    }
}
