using AutoMapper;
using Domain;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Personnel Related
            CreateMap<Personnel, PersonnelDto>()
            .ForMember(destination => destination.Sex, option => option.MapFrom(source => source.Sex.ToString()))
            .ForMember(destination => destination.Photo, option => option.MapFrom(source => source.Photo.Url));

            CreateMap<Personnel, PersonnelPayrollDto>()
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
            .ForMember(destination => destination.ActualDeductions, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().ActualDeductions))
            .ForMember(destination => destination.NetPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))
            .ForMember(destination => destination.Platform, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Platform))
            .ForMember(destination => destination.IsPayrollVariablesSet, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().IsVariablesSet));

            //Payroll Related
            CreateMap<Payroll, PersonnelPayrollDto>()
            .ForMember(destination => destination.PersonnelName, option => option.MapFrom(source => source.Personnel.Name))
            .ForMember(destination => destination.PersonnelId, option => option.MapFrom(source => source.Id))
            .ForMember(destination => destination.PersonnelPhoto, option => option.MapFrom(source => source.Personnel.Photo.Url))
            .ForMember(destination => destination.PhoneNumber, option => option.MapFrom(source => source.Personnel.PhoneNumber))
            .ForMember(destination => destination.IsPayrollVariablesSet, option => option.MapFrom(source => source.IsVariablesSet))
            .ForMember(destination => destination.Period, option => option.MapFrom(source => source.Date.ToFormalMonthAndYear()));


            //Deduction Related
            CreateMap<Deduction, DeductionDto>()
            .ForMember(destination => destination.DateAdded, option => option.MapFrom(source => source.DateAdded.ToFormalShortDate()))
            .ForMember(destination => destination.LastModified, option => option.MapFrom(source => source.LastModified.ToFormalShortDateWithTime()))
            .ForMember(destination => destination.AddedBy, option => option.MapFrom(source => source.AddedBy.Name))
            .ForMember(destination => destination.ModifiedBy, option => option.MapFrom(source => source.ModifiedBy.Name));
        }
    }
}