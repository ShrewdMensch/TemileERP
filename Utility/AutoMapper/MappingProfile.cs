using AutoMapper;
using Domain;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Personnel, PersonnelDto>()
            .ForMember(destination => destination.Sex, option => option.MapFrom(message => message.Sex.ToString()))
            .ForMember(destination => destination.Photo, option => option.MapFrom(source => source.Photo.Url));
        }
    }
}