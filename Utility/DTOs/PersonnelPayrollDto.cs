using System;

namespace Utility.DTOs
{
    public class PersonnelPayrollDto
    {
        public Guid PersonnelId { get; set; }
        public Guid PayrollId { get; set; }
        public string PersonnelName { get; set; }
        public double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public string Platform { get; set; }
        public double TotalPay { get; set; }
        public double ActualDeductions { get; set; }
        public float TotalDeductions { get; set; }
        public double NetPay { get; set; }
        public bool IsPayrollVariablesSet { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonnelPhoto { get; set; }
        public string Period { get; set; }
    }
}