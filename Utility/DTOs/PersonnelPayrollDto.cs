using System;
using System.Collections.Generic;

namespace Utility.DTOs
{
    public class PersonnelPayrollDto
    {
        public string PersonnelId { get; set; }
        public string PayrollId { get; set; }
        public string PersonnelName { get; set; }
        public string DaysWorked { get; set; }
        public double DailyRate { get; set; }
        public string DailyRateInCurrency { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool WorkedWeekend { get; set; }
        public string Vessel { get; set; }
        public double NetPay { get; set; }
        public string NetPayInCurrency { get; set; }
        public double GrossPay { get; set; }
        public string GrossPayInCurrency { get; set; }
        public double TotalDeductedAmount { get; set; }
        public float TotalDeductedPercentage { get; set; }
        public bool IsPayrollVariablesSet { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonnelPhoto { get; set; }
        public string Period { get; set; }
        public IEnumerable<AllowanceDto> Allowances { get; set; }
        public IEnumerable<SpecificDeductionDto> SpecificDeductions { get; set; }
        public IEnumerable<ArrearDto> Arrears { get; set; }
    }
}