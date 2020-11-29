using AutoMapper;
using Domain;
using Utility.DTOs;

namespace Utility.AutoMapper
{
    public class AllowanceAndDeductionMappingProfile : Profile
    {
        public AllowanceAndDeductionMappingProfile()
        {
            CreateMap<Deduction, DeductionDto>()
                .ForMember(destination => destination.DateAdded,
                option => option.MapFrom(source => source.DateAdded.ToFormalShortDateWithTime()))

                .ForMember(destination => destination.LastModified,
                option => option.MapFrom(source => source.LastModified.ToFormalShortDateWithTime()))

                .ForMember(destination => destination.AddedBy,
                option => option.MapFrom(source => source.AddedBy.Name))

                .ForMember(destination => destination.ModifiedBy,
                option => option.MapFrom(source => source.ModifiedBy.Name));



            CreateMap<DeductionDetail, DeductionDetailDto>()
                .ForMember(destination => destination.DeductedPercentage,
                option => option.MapFrom(source => source.DeductedPercentage.ToPercentageStr()))

                .ForMember(destination => destination.DeductedAmount,
                option => option.MapFrom(source => source.DeductedAmount.ToCurrency()));


            CreateMap<SpecificDeduction, SpecificDeductionDto>()
                .ForMember(destination=>destination.AmountInCurrency, 
                option=>option.MapFrom(source=>source.Amount.ToCurrency()));

            CreateMap<Allowance, AllowanceDto>()
                .ForMember(destination => destination.AmountInCurrency,
                option => option.MapFrom(source => source.Amount.ToCurrency()));
        }
    }
}
