using AutoMapper;
using CommVill.DTO;
using CommVill.Models;

namespace CommVill.DAL.Helper
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<User, UserDto>();
        } 
    }
}
