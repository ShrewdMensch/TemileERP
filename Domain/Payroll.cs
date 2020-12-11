using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Utility;

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
        public int DaysWorked => GetDaysWorked();
        public double GrossPay => DailyRate * DaysWorked;
        public double NetPay => GetNetPay();
        public float TotalDeductedPercentage { get; set; }
        public double TotalDeductedAmount => GetTotalDeductedAmount();
        public double TotalEarnings => GetTotalEarnings();
        public DateTime Date { get; set; }
        public bool IsVariablesSet { get; set; }
        public string PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }
        public string PersonnelDesignation { get; set; }
        public string Vessel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool WorkedWeekend { get; set; }
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
        
        private double GetTotalEarnings()
        {
            var totalAllowances = Allowances.Sum(s => s.Amount);

            return GrossPay + totalAllowances;
        }
        private double GetTotalDeductedAmount()
        {
            var totalSpecificDeductions = SpecificDeductions.Sum(s => s.Amount);
            var totalDeductions = (GrossPay * (TotalDeductedPercentage / 100))  + totalSpecificDeductions;

            return totalDeductions;
        }

        private int GetDaysWorked()
        {
            if (WorkedWeekend)
                return StartDate.AllDaysUntil(EndDate);

            return StartDate.BusinessDaysUntil(EndDate);
        }
    }
}