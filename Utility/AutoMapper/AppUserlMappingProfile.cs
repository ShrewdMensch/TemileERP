﻿using AutoMapper;
using Domain;
using Utility.DTOs;
using Utility.Extensions;

namespace Utility.AutoMapper
{
    public class AppUserlMappingProfile : Profile
    {
        public AppUserlMappingProfile()
        {
            CreateMap<AppUser, AppUserDto>()
                .ForMember(destination => destination.DateRegistered, 
                option => option.MapFrom(source => source.DateOfRegistration.ToFormalShortDate()));
        }
    }
}
