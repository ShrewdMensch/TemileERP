using AutoMapper;
using Domain;
using System;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class PaymentSlipMappingProfile : Profile
    {
        public PaymentSlipMappingProfile()
        {
            CreateMap<Payroll, PaymentSlipDto>()
                .ForMember(destination => destination.PersonnelName,
                option => option.MapFrom(source => source.Personnel.Name))

                .ForMember(destination => destination.PersonnelId,
                option => option.MapFrom(source => source.Personnel.Id))

                .ForMember(destination => destination.PersonnelDesignation,
                option => option.MapFrom(source => source.PersonnelDesignation))

                .ForMember(destination => destination.PersonnelDateJoined,
                option => option.MapFrom(source => source.Personnel.DateJoined.ToFormalShortDate()))

                .ForMember(destination => destination.Bank,
                option => option.MapFrom(source => source.PaymentDetail.Bank))

                .ForMember(destination => destination.AccountName,
                option => option.MapFrom(source => source.PaymentDetail.AccountName))

                .ForMember(destination => destination.AccountNumber,
                option => option.MapFrom(source => source.PaymentDetail.AccountNumber))

                .ForMember(destination => destination.DailyRate,
                option => option.MapFrom(source => source.DailyRate.ToCurrency()))
                
                .ForMember(destination => destination.DaysWorked,
                option => option.MapFrom(source => source.DaysWorked.ToDays()))

                .ForMember(destination => destination.GrossPay,
                option => option.MapFrom(source => source.GrossPay.ToCurrency()))

                .ForMember(destination => destination.NetPay,
                option => option.MapFrom(source => source.NetPay.ToCurrency()))

                .ForMember(destination => destination.NetPayRaw,
                option => option.MapFrom(source => Convert.ToInt32(source.NetPay)))

                .ForMember(destination => destination.Date,
                option => option.MapFrom(source => source.Date.ToFormalMonthAndYear()))

                .ForMember(destination => destination.StartDate,
                option => option.MapFrom(source => source.StartDate.ToFormalShortDate()))
                
                .ForMember(destination => destination.EndDate,
                option => option.MapFrom(source => source.EndDate.ToFormalShortDate()))

                .ForMember(destination => destination.Deductions,
                option => option.MapFrom(source => source.DeductionDetails))

                .ForMember(destination => destination.TotalDeductedAmount,
                option => option.MapFrom(source => source.TotalDeductedAmount.ToCurrency()))
                
                .ForMember(destination => destination.BasicSalary,
                option => option.MapFrom(source => source.BasicSalary.ToCurrency()))
                
                .ForMember(destination => destination.TotalArrears,
                option => option.MapFrom(source => source.TotalArrears.ToCurrency()))

                .ForMember(destination => destination.TotalDeductedPercentage,
                option => option.MapFrom(source => source.TotalDeductedPercentage.ToPercentageStr()))

                .ForMember(destination => destination.PaymentSlipNumber,
                option => option.MapFrom(source => source.Id))

                .ForMember(destination => destination.Period,
                option => option.MapFrom(source => source.StartDate.ToFormalShortDate().CombineAsDateRange(source.EndDate.ToFormalShortDate())));
        }
    }
}
