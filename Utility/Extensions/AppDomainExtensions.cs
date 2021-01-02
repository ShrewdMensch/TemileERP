using System.Collections.Generic;
using System;
using System.Linq;
using Domain;
using Utility.DTOs;
using Utility.Comparer;

namespace Utility.Extensions
{
    public static class AppDomainExtensions
    {

        public static Payroll GetCurrentPayroll(this IEnumerable<Payroll> payrolls) => payrolls.FirstOrDefault(
            p => p.Date.ToFormalMonthAndYear() == DateTime.Today.ToFormalMonthAndYear());

        public static IEnumerable<InstructionToBankListDto> DistinctByVessel(this IEnumerable<InstructionToBankListDto> instructionToBankDtos) 
            => instructionToBankDtos.Distinct(new InstructionToBankListDtoComparer());

        public static IEnumerable<Payroll> GetPastPayrolls(this Personnel personnel)
        {
            var previousPayrolls = personnel.Payrolls.Where(p => p.Date.ToFormalMonthAndYear() != DateTime.Today.ToFormalMonthAndYear());

            return previousPayrolls;
        }
    }

}