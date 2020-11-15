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
            .ForMember(destination => destination.Sex, option => option.MapFrom(source => source.Sex.ToString()))
            .ForMember(destination => destination.Photo, option => option.MapFrom(source => source.Photo.Url));

            CreateMap<Personnel, PersonnelCurrentPayrollDto>()
            .ForMember(destination => destination.PersonnelName, option => option.MapFrom(source => source.Name))
            .ForMember(destination => destination.PersonnelId, option => option.MapFrom(source => source.Id))
            .ForMember(destination => destination.PersonnelPhoto, option => option.MapFrom(source => source.Photo.Url))
            .ForMember(destination => destination.PhoneNumber, option => option.MapFrom(source => source.PhoneNumber))
            .ForMember(destination => destination.PayrollId, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Id))
            .ForMember(destination => destination.DailyRate, option => option.MapFrom(source => source.DailyRate))
            .ForMember(destination => destination.DaysWorked, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().DaysWorked))
            .ForMember(destination => destination.TotalPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalPay))
            .ForMember(destination => destination.NetPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))
            .ForMember(destination => destination.TotalDeductions, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductions))
            .ForMember(destination => destination.NetPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))
            .ForMember(destination => destination.Platform, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Platform))
            .ForMember(destination => destination.IsPayrollVariablesSet, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().IsVariablesSet));
        }
    }
}