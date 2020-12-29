using AutoMapper;
using Domain;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class PersonnelMappingProfile : Profile
    {
        public PersonnelMappingProfile()
        {
            _ = CreateMap<Personnel, PersonnelDto>()
                .ForMember(destination => destination.Sex,
                option => option.MapFrom(source => source.Sex.ToString()))

                .ForMember(destination => destination.DateJoined,
                option => option.MapFrom(source => source.DateJoined.ToFormalShortDate()))

                .ForMember(destination => destination.DailyRateStr,
                option => option.MapFrom(source => source.DailyRate.ToCurrency()))

                .ForMember(destination => destination.Photo,
                option => option.MapFrom(source => source.Photo.Url))

                .ForMember(destination => destination.DateLeft, option =>
                    option.MapFrom(source => source.DateLeft == null ? "N/A" : source.DateLeft.GetValueOrDefault().ToFormalShortDate()));



            CreateMap<Personnel, PersonnelPayrollDto>()
                .ForMember(destination => destination.PersonnelName,
                option => option.MapFrom(source => source.Name))

                .ForMember(destination => destination.PersonnelFullName,
                option => option.MapFrom(source => source.FullName))

                .ForMember(destination => destination.PersonnelDesignation,
                option => option.MapFrom(source => source.Designation))

                .ForMember(destination => destination.IsPersonnelActive,
                option => option.MapFrom(source => source.IsActive))

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

                .ForMember(destination => destination.DailyRateInCurrency,
                option => option.MapFrom(source => source.DailyRate.ToCurrency()))

                .ForMember(destination => destination.DaysWorked,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().DaysWorked.ToDays()))

                .ForMember(destination => destination.GrossPay,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().GrossPay))

                .ForMember(destination => destination.NetPay,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))

                .ForMember(destination => destination.NetPayInCurrency,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay.ToCurrency()))

                .ForMember(destination => destination.GrossPayInCurrency,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().GrossPay.ToCurrency()))

                .ForMember(destination => destination.TotalDeductedPercentage,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductedPercentage))

                .ForMember(destination => destination.TotalDeductedAmount,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductedAmount))

                .ForMember(destination => destination.Allowances,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Allowances))

                .ForMember(destination => destination.SpecificDeductions,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().SpecificDeductions))

                .ForMember(destination => destination.Arrears,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Arrears))

                .ForMember(destination => destination.Vessel,
                option => option.MapFrom(source => source.Vessel))

                .ForMember(destination => destination.Period,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().StartDate.ToFormalShortDate()
                .CombineAsDateRange(source.Payrolls.GetCurrentPayroll().EndDate.ToFormalShortDate())))

                .ForMember(destination => destination.StartDate,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().StartDate.ToShortDate()))

                .ForMember(destination => destination.EndDate,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().EndDate.ToShortDate()))

                .ForMember(destination => destination.DateLeft, option =>
                {
                    option.PreCondition(source => source.DateLeft != null);
                    option.MapFrom(source => source.DateLeft.GetValueOrDefault().ToFormalShortDate());
                })

                .ForMember(destination => destination.DateLeft, option =>
                    option.MapFrom(source => source.DateLeft == null ? "N/A" : source.DateLeft.GetValueOrDefault().ToFormalShortDate()))

                .ForMember(destination => destination.WorkedWeekend,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().WorkedWeekend))

                .ForMember(destination => destination.IsPayrollVariablesSet,
                option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().IsVariablesSet));

        }
    }
}
