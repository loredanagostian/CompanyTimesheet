using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Timesheet.API.DbContexts;
using Timesheet.API.Entities;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;

namespace Timesheet.API.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly TimesheetContext _context;
        private readonly IMapper _mapper;

        public TimeEntryRepository(TimesheetContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TimeEntryModel>> GetTimeEntriesAsync()
        {
            var timeEntries = await _context.TimeEntries.ToListAsync();

            return _mapper.Map<IEnumerable<TimeEntryModel>>(timeEntries);
        }

        public async Task<(TimeEntryModel, TimeEntry)> CreateTimeEntryAsync(CreateTimeEntryDto timeEntryDto)
        {
            var timeEntryEntity = _mapper.Map<TimeEntry>(timeEntryDto);
            await _context.TimeEntries.AddAsync(timeEntryEntity);
            await _context.SaveChangesAsync();

            return (_mapper.Map<TimeEntryModel>(timeEntryEntity), timeEntryEntity);
        }

        public async Task<IEnumerable<TimeEntryModel>> GetTimeEntriesByEmployeeIdAsync(int id)
        {
            var timeEntries = await _context.TimeEntries
                .Where(t => t.EmployeeId == id)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TimeEntryModel>>(timeEntries);
        }
    }
}
