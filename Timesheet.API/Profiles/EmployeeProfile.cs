using AutoMapper;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        { 
            CreateMap<CreateEmployeeDto, Employee>();
        }
    }
}
