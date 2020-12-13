using Domain.Utility;
using System;

namespace Domain
{
    public class Arrear
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysWorked => GetDaysWorked();
        public double Amount => GetAmount();
        public string AffectedPayrollId { get; set; }
        public virtual Payroll AffectedPayroll { get; set; }
        public string CorrectivePayrollId { get; set; }
        public virtual Payroll CorrectivePayroll { get; set; }

        private int GetDaysWorked()
        {
            if (AffectedPayroll.WorkedWeekend)
                return StartDate.AllDaysUntil(EndDate);

            return StartDate.BusinessDaysUntil(EndDate);
        }

        private double GetAmount()
        {
            return AffectedPayroll.DailyRate * DaysWorked;
        }
    }

}
