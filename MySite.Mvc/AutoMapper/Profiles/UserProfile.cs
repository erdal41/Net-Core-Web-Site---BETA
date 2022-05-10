using AutoMapper;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using MySite.Mvc.Areas.Admin.Models;

namespace MySite.Mvc.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserAddViewModel, User>();
        }
    }
}