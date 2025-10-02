using AutoMapper;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Profiles
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            CreateMap<CreateUserAccountDto, UserAccount>();
        }
    }
}
