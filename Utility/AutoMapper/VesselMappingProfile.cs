using AutoMapper;
using Domain;
using Utility.DTOs;
using Utility.Extensions;

namespace Utility.AutoMapper
{
    public class VesselMappingProfile : Profile
    {
        public VesselMappingProfile()
        {
            CreateMap<Vessel, Select2InputDto>()
                .ForMember(destination => destination.Text, option => option.MapFrom(p => p.Name))
                .ForMember(destination => destination.Id, option => option.MapFrom(p => p.Name));

            CreateMap<Vessel, VesselDto>()
                    .ForMember(destination => destination.DateAdded,
                    option => option.MapFrom(source => source.DateAdded.ToFormalShortDateWithTime()))

                    .ForMember(destination => destination.LastModified,
                    option => option.MapFrom(source => source.LastModified.ToFormalShortDateWithTime()))

                    .ForMember(destination => destination.AddedBy,
                    option => option.MapFrom(source => source.AddedBy.Name))

                    .ForMember(destination => destination.ModifiedBy,
                    option => option.MapFrom(source => source.ModifiedBy.Name));
        }
    }
}
