using System;

namespace Utility.DTOs
{
    public class PersonnelPayrollDto
    {
        public string PersonnelId { get; set; }
        public string PayrollId { get; set; }
        public string PersonnelName { get; set; }
        public double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool WorkedWeekend { get; set; }
        public string Vessel { get; set; }
        public double GrossPay { get; set; }
        public double TotalDeductedAmount { get; set; }
        public float TotalDeductedPercentage { get; set; }
        public double NetPay { get; set; }
        public bool IsPayrollVariablesSet { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonnelPhoto { get; set; }
        public string Period { get; set; }
    }
}