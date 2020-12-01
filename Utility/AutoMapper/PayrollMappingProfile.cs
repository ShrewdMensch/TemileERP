using AutoMapper;
using Domain;
using System;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class PayrollMappingProfile : Profile
    {
        public PayrollMappingProfile()
        {
            CreateMap<Payroll, PersonnelPayrollDto>()
                .ForMember(destination => destination.PersonnelName,
                option => option.MapFrom(source => source.Personnel.Name))

                .ForMember(destination => destination.PersonnelId,
                option => option.MapFrom(source => source.Personnel.Id))

                .ForMember(destination => destination.PayrollId,
                option => option.MapFrom(source => source.Id))

                .ForMember(destination => destination.PersonnelPhoto,
                option => option.MapFrom(source => source.Personnel.Photo.Url))

                .ForMember(destination => destination.PhoneNumber,
                option => option.MapFrom(source => source.Personnel.PhoneNumber))

                .ForMember(destination => destination.IsPayrollVariablesSet,
                option => option.MapFrom(source => source.IsVariablesSet))

                .ForMember(destination => destination.DaysWorked,
                option => option.MapFrom(source => source.DaysWorked.ToDays()))
                
                .ForMember(destination => destination.DailyRateInCurrency,
                option => option.MapFrom(source => source.DailyRate.ToCurrency()))
                
                .ForMember(destination => destination.GrossPayInCurrency,
                option => option.MapFrom(source => source.GrossPay.ToCurrency()))
                
                .ForMember(destination => destination.NetPayInCurrency,
                option => option.MapFrom(source => source.NetPay.ToCurrency()))

               .ForMember(destination => destination.Period,
                option => option.MapFrom(source => source.StartDate.ToFormalShortDate().CombineAsDateRange(source.EndDate.ToFormalShortDate())))

                .ForMember(destination => destination.StartDate,
                option => option.MapFrom(source => source.StartDate.ToShortDate()))
                
                .ForMember(destination => destination.EndDate,
                option => option.MapFrom(source => source.EndDate.ToShortDate()));
        }
    }
}
