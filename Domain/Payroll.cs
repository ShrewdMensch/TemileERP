using System;
using System.Collections.Generic;

namespace Domain
{
    public class Payroll
    {
        public Payroll()
        {
            Date = DateTime.Now;
            IsVariablesSet = true;
        }

        public string Id { get; set; }
        public Double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public Double GrossPay => DailyRate * DaysWorked;
        public Double NetPay => GrossPay - TotalDeductedAmount;
        public float TotalDeductedPercentage { get; set; }
        public double TotalDeductedAmount => GrossPay * (TotalDeductedPercentage / 100);
        public string Vessel { get; set; }
        public DateTime Date { get; set; }
        public bool IsVariablesSet { get; set; }
        public string PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }
        public virtual ICollection<DeductionDetail> DeductionDetails { get; set; }
        public virtual PaymentDetail PaymentDetail { get; set; }
    }
}