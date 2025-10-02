using AutoMapper;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Profiles
{
    public class TimeEntryProfile : Profile
    {
        public TimeEntryProfile()
        {
            CreateMap<CreateTimeEntryDto, TimeEntry>();
        }
    }
}
