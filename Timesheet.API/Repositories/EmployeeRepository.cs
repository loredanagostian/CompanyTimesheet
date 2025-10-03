//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Models;
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

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.UserAccounts)
                .Include(e => e.TimeEntries)
                .AsNoTracking()
                .ToListAsync();

            return employees;
        }

        public async Task CreateEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            var entity = await _context.Employees
                .Include(e => e.UserAccounts)
                .Include(e => e.TimeEntries)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            return entity;
        }

        //public async Task<Employee?> FindEmployeeByIdAsync(int id)
        //{
        //    return await _context.Employees.FindAsync(id);
        //}

        public async Task DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateEmployeeTimeEntriesAsync(TimeEntry timeEntry)
        //{
        //    var employee = await FindEmployeeByIdAsync(timeEntry.EmployeeId);

        //    if (employee != null)
        //    {
        //        if (employee.TimeEntries == null)
        //            employee.TimeEntries = new List<TimeEntry>();

        //        if (!employee.TimeEntries.Contains(timeEntry))
        //            employee.TimeEntries.Add(timeEntry);

        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<Employee?> GetEmployeeByCNP(string cnp)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CNP == cnp);

            return employee;
        }
    }
}
