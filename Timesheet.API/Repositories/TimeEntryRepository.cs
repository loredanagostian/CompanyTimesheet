//using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly TimesheetContext _context;
        //private readonly IMapper _mapper;

        public TimeEntryRepository(TimesheetContext context /*, IMapper mapper*/)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResult<IEnumerable<TimeEntry>>> GetTimeEntries()
        {
            var timeEntries = await _context.TimeEntries
                .Include(te => te.Employee)
                .AsNoTracking()
                .ToListAsync();

            return ServiceResult<IEnumerable<TimeEntry>>.Success(timeEntries);
        }

        public async Task CreateTimeEntry(TimeEntry timeEntry)
        {
            await _context.TimeEntries.AddAsync(timeEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<TimeEntry?> GetTimeEntryById(int id)
        {
            return await _context.TimeEntries.FindAsync(id);
        }

        public async Task DeleteTimeEntry(TimeEntry timeEntry)
        {
            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();
        }
    }
}
