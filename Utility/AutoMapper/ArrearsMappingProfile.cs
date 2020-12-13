using AutoMapper;
using Domain;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class ArrearsMappingProfile : Profile
    {
        public ArrearsMappingProfile()
        {
            CreateMap<Arrear, ArrearDto>()
                .ForMember(destination => destination.Period,
                option => option.MapFrom(p => p.StartDate.ToFormalShortDate().CombineAsDateRange(p.EndDate.ToFormalShortDate())))
                .ForMember(destination => destination.PersonnelDailyRate, option => option.MapFrom(p => p.AffectedPayroll.DailyRate.ToCurrency()))
                .ForMember(destination => destination.PersonnelDesignation, option => option.MapFrom(p => p.AffectedPayroll.PersonnelDesignation))
                .ForMember(destination => destination.Vessel, option => option.MapFrom(p => p.AffectedPayroll.Vessel))
                .ForMember(destination => destination.StartDate, option => option.MapFrom(p => p.StartDate.ToShortDate()))
                .ForMember(destination => destination.EndDate, option => option.MapFrom(p => p.EndDate.ToShortDate()))
                .ForMember(destination => destination.Amount, option => option.MapFrom(p => p.Amount.ToCurrency()))
                .ForMember(destination => destination.WorkedWeekend, option => option.MapFrom(p => p.AffectedPayroll.WorkedWeekend))
                .ForMember(destination => destination.DaysWorked, option => option.MapFrom(p => p.DaysWorked.ToDays()));


        }
    }
}
