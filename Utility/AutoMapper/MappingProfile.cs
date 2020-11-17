using System;
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
            .ForMember(destination => destination.GrossPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().GrossPay))
            .ForMember(destination => destination.NetPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))
            .ForMember(destination => destination.TotalDeductedPercentage, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductedPercentage))
            .ForMember(destination => destination.TotalDeductedAmount, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().TotalDeductedAmount))
            .ForMember(destination => destination.NetPay, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().NetPay))
            .ForMember(destination => destination.Vessel, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().Vessel))
            .ForMember(destination => destination.IsPayrollVariablesSet, option => option.MapFrom(source => source.Payrolls.GetCurrentPayroll().IsVariablesSet));

            //Payroll Related
            CreateMap<Payroll, PersonnelPayrollDto>()
            .ForMember(destination => destination.PersonnelName, option => option.MapFrom(source => source.Personnel.Name))
            .ForMember(destination => destination.PersonnelId, option => option.MapFrom(source => source.Personnel.Id))
            .ForMember(destination => destination.PayrollId, option => option.MapFrom(source => source.Id))
            .ForMember(destination => destination.PersonnelPhoto, option => option.MapFrom(source => source.Personnel.Photo.Url))
            .ForMember(destination => destination.PhoneNumber, option => option.MapFrom(source => source.Personnel.PhoneNumber))
            .ForMember(destination => destination.IsPayrollVariablesSet, option => option.MapFrom(source => source.IsVariablesSet))
            .ForMember(destination => destination.Period, option => option.MapFrom(source => source.Date.ToFormalMonthAndYear()));

            CreateMap<Payroll, PaymentSlipDto>()
            .ForMember(destination => destination.PersonnelName, option => option.MapFrom(source => source.Personnel.Name))
            .ForMember(destination => destination.PersonnelId, option => option.MapFrom(source => source.Personnel.Id))
            .ForMember(destination => destination.PersonnelDesignation, option => option.MapFrom(source => source.Personnel.Designation))
            .ForMember(destination => destination.PersonnelDateJoined, option => option.MapFrom(source => source.Personnel.DateJoined.ToFormalShortDate()))

            .ForMember(destination => destination.Bank, option => option.MapFrom(source => source.PaymentDetail.Bank))
            .ForMember(destination => destination.AccountName, option => option.MapFrom(source => source.PaymentDetail.AccountName))
            .ForMember(destination => destination.AccountNumber, option => option.MapFrom(source => source.PaymentDetail.AccountNumber))
            .ForMember(destination => destination.BVN, option => option.MapFrom(source => source.PaymentDetail.BVN))

            .ForMember(destination => destination.DailyRate, option => option.MapFrom(source => source.DailyRate.ToCurrency()))
            .ForMember(destination => destination.GrossPay, option => option.MapFrom(source => source.GrossPay.ToCurrency()))
            .ForMember(destination => destination.NetPay, option => option.MapFrom(source => source.NetPay.ToCurrency()))
            .ForMember(destination => destination.NetPayRaw, option => option.MapFrom(source => Convert.ToInt32(source.NetPay)))
            .ForMember(destination => destination.Date, option => option.MapFrom(source => source.Date.ToFormalMonthAndYear()))
            .ForMember(destination => destination.Deductions, option => option.MapFrom(source => source.DeductionDetails))
            .ForMember(destination => destination.TotalDeductedAmount, option => option.MapFrom(source => source.TotalDeductedAmount.ToCurrency()))
            .ForMember(destination => destination.TotalDeductedPercentage, option => option.MapFrom(source => source.TotalDeductedPercentage.ToPercentageStr()))
            .ForMember(destination => destination.PaymentSlipNumber, option => option.MapFrom(source => source.Id));


            //Deduction Related
            CreateMap<Deduction, DeductionDto>()
            .ForMember(destination => destination.DateAdded, option => option.MapFrom(source => source.DateAdded.ToFormalShortDate()))
            .ForMember(destination => destination.LastModified, option => option.MapFrom(source => source.LastModified.ToFormalShortDateWithTime()))
            .ForMember(destination => destination.AddedBy, option => option.MapFrom(source => source.AddedBy.Name))
            .ForMember(destination => destination.ModifiedBy, option => option.MapFrom(source => source.ModifiedBy.Name));

            CreateMap<DeductionDetail, DeductionDetailDto>()
            .ForMember(destination => destination.DeductedPercentage, option => option.MapFrom(source => source.DeductedPercentage.ToPercentageStr()))
            .ForMember(destination => destination.DeductedAmount, option => option.MapFrom(source => source.DeductedAmount.ToCurrency()));

        }
    }
}