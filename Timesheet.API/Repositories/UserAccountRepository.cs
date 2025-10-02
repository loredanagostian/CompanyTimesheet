using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Entities;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly TimesheetContext _context;
        private readonly IMapper _mapper;

        public UserAccountRepository(TimesheetContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> DeleteUserAccountsByEmployeeIdAsync(int id)
        {
            var accounts = await _context.UserAccounts
               .Where(u => u.EmployeeId == id)
               .ToListAsync();

            if (accounts.Count == 0) return 0;

            _context.UserAccounts.RemoveRange(accounts);

            return await _context.SaveChangesAsync();
        }

        public async Task<(UserAccountModel?, UserAccount?)> CreateUserAccountAsync(CreateUserAccountDto userAccountDto)
        {
            var userAccountEntity = _mapper.Map<UserAccount>(userAccountDto);

            var userAccountFound = await _context.UserAccounts.FindAsync(userAccountDto.Email);

            if (userAccountFound != null)
            {
                return (null, null);
            }

            _context.UserAccounts.Add(userAccountEntity);
            await _context.SaveChangesAsync();

            return (_mapper.Map<UserAccountModel>(userAccountEntity), userAccountEntity);
        }

        public async Task<IEnumerable<UserAccountModel>> GetUserAccountsAsync()
        {
            var userAccounts = await _context.UserAccounts.ToListAsync();
            return _mapper.Map<IEnumerable<UserAccountModel>>(userAccounts);
        }
    }
}
