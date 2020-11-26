using System;
using System.Collections.Generic;
using System.Linq;

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
        public double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public double GrossPay => DailyRate * DaysWorked;
        public double NetPay => GetNetPay();
        public float TotalDeductedPercentage { get; set; }
        public double TotalDeductedAmount => GetTotalDeductedAmount();
        public DateTime Date { get; set; }
        public bool IsVariablesSet { get; set; }
        public string PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }
        public string Vessel { get; set; }
        public virtual ICollection<Allowance> Allowances { get; set; }
        public virtual ICollection<SpecificDeduction> SpecificDeductions { get; set; }
        public virtual ICollection<DeductionDetail> DeductionDetails { get; set; }
        public virtual PaymentDetail PaymentDetail { get; set; }

        private double GetNetPay()
        {
            var totalAllowances = Allowances.Sum(s => s.Amount);
            var netPay = (GrossPay + totalAllowances) - (TotalDeductedAmount);

            return netPay;
        }
        private double GetTotalDeductedAmount()
        {
            var totalSpecificDeductions = SpecificDeductions.Sum(s => s.Amount);
            var totalDeductions = (GrossPay * (TotalDeductedPercentage / 100))  + totalSpecificDeductions;

            return totalDeductions;
        }
    }
}