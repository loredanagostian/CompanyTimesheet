//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Models;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly TimesheetContext _context;
        //private readonly IMapper _mapper;

        public UserAccountRepository(TimesheetContext context /*, IMapper mapper*/)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResult<IEnumerable<UserAccount>>> GetUserAccounts()
        {
            var userAccounts = await _context.UserAccounts
                .AsNoTracking()
                .ToListAsync();

            return ServiceResult<IEnumerable<UserAccount>>.Success(userAccounts);
        }

        public async Task CreateUserAccount(UserAccount userAccount)
        {
            await _context.UserAccounts.AddAsync(userAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccountsByEmployeeId(int employeeId)
        {
            return await _context.UserAccounts
                .Where(ua => ua.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<UserAccount?> GetUserAccountById(int id)
        {
            return await _context.UserAccounts.FindAsync(id);
        }

        public async Task DeleteUserAccount(UserAccount userAccount)
        {
            _context.UserAccounts.Remove(userAccount);
            await _context.SaveChangesAsync();
        }
    }
}
