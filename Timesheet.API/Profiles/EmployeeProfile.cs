using AutoMapper;

namespace Timesheet.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        { 
            CreateMap<Entities.Employee, Models.EmployeeModel>();
        }
    }
}
