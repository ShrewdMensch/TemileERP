using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class InstructionToBankMappingProfile : Profile
    {
        public InstructionToBankMappingProfile()
        {
            CreateMap<Payroll, InstructionToBankDetailsDto>()
                .ForMember(destination => destination.PersonnelName, 
                option => option.MapFrom(source => source.Personnel.FullName))

                .ForMember(destination => destination.PersonnelId, 
                option => option.MapFrom(source => source.Personnel.Id))

                .ForMember(destination => destination.PayrollId, 
                option => option.MapFrom(source => source.Id))

                .ForMember(destination => destination.Bank, 
                option => option.MapFrom(source => source.PaymentDetail.Bank))

                .ForMember(destination => destination.AccountName, 
                option => option.MapFrom(source => source.PaymentDetail.AccountName))

                .ForMember(destination => destination.AccountNumber, 
                option => option.MapFrom(source => source.PaymentDetail.AccountNumber))

                .ForMember(destination => destination.NetPay, 
                option => option.MapFrom(source => source.NetPay.ToCurrency()));
        }
    }
}
