using AutoMapper;
using MySite.Entities.Concrete;
using MySite.Entities.Dtos;
using System;

namespace MySite.Services.AutoMapper.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagAddDto, Tag>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<TagUpdateDto, Tag>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Tag, TagUpdateDto>();
        }
    }
}