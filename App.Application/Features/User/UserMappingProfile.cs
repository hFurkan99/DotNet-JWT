using App.Application.Features.User.Dto;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserApp, UserAppDto>().ReverseMap();
        }
    }
}
