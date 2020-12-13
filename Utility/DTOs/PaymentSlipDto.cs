using System.Collections.Generic;

namespace Utility.DTOs
{
    public class PaymentSlipDto
    {
        public string PaymentSlipNumber { get; set; }
        public string PersonnelName { get; set; }
        public string PersonnelId { get; set; }
        public string PersonnelDesignation { get; set; }
        public string PersonnelDateJoined { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string TotalDeductedPercentage { get; set; }
        public string TotalDeductedAmount { get; set; }
        public string TotalEarnings { get; set; }
        public string TotalArrears { get; set; }
        public string DailyRate { get; set; }
        public string DaysWorked { get; set; }
        public string GrossPay { get; set; }
        public string NetPay { get; set; }
        public int NetPayRaw { get; set; }
        public string Vessel { get; set; }
        public string Date { get; set; }
        public string Period { get; set; }
        public bool WorkedWeekend { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IEnumerable<DeductionDetailDto> Deductions { get; set; }
        public IEnumerable<AllowanceDto> Allowances { get; set; }
        public IEnumerable<SpecificDeductionDto> SpecificDeductions { get; set; }
        public IEnumerable<ArrearDto> Arrears { get; set; }
    }
}