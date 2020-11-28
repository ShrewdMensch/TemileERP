using AutoMapper;
using Domain;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class PersonnelMappingProfile : Profile
    {
        public PersonnelMappingProfile()
        {
            CreateMap<Personnel, PersonnelDto>()
                .ForMember(destination => destination.Sex, 
                option => option.MapFrom(source => source.Sex.ToString()))

                .ForMember(destination => destination.DateJoined, 
                option => option.MapFrom(source => source.DateJoined.ToFormalShortDate()))

                .ForMember(destination => destination.DailyRateStr, 
                option => option.MapFrom(source => source.DailyRate.ToCurrency()))

                .ForMember(destination => destination.Photo, 
                option => option.MapFrom(source => source.Photo.Url));



            CreateMap<Personnel, PersonnelPayrollDto>()
                .ForMember(destination => destination.PersonnelName, 
                option => option.MapFrom(source => source.Name))

                .ForMember(destination => destination.PersonnelId, 
                option => option.MapFrom(source => source.Id))

                .ForMember(destination => destination.PersonnelPhoto, 
                option => option.MapFrom(source => source.Photo.Url))

                .ForMember(destination => destination.PhoneNumber, 
                option => option.MapFrom(source => source.PhoneNumber))

                .ForMember(destination => destination.PayrollId, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Id))

                .ForMember(destination => destination.DailyRate, 
                option => option.MapFrom(source => source.DailyRate))

                .ForMember(destination => destination.DaysWorked, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().DaysWorked))

                .ForMember(destination => destination.GrossPay, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().GrossPay))

                .ForMember(destination => destination.NetPay, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))

                .ForMember(destination => destination.TotalDeductedPercentage, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductedPercentage))

                .ForMember(destination => destination.TotalDeductedAmount, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductedAmount))

                .ForMember(destination => destination.NetPay, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))

                .ForMember(destination => destination.Vessel, 
                option => option.MapFrom(source => source.Vessel))

                .ForMember(destination => destination.Period, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().StartDate.ToFormalShortDate()
                .CombineAsRange(source.Payrolls.GetCurrentPayroll().EndDate.ToFormalShortDate())))

                .ForMember(destination => destination.StartDate, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().StartDate.ToShortDate()))
                
                .ForMember(destination => destination.EndDate, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().EndDate.ToShortDate()))
                
                .ForMember(destination => destination.WorkedWeekend, 
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().WorkedWeekend))

                .ForMember(destination => destination.IsPayrollVariablesSet,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().IsVariablesSet));

        }
    }
}
