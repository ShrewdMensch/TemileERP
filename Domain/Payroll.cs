using System;

namespace Domain
{
    public class Payroll
    {
        public Payroll()
        {
            Date = DateTime.Now;
            IsVariablesSet = false;
        }

        public Guid Id { get; set; }
        public Double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public Double TotalPay => DailyRate * DaysWorked;
        public Double NetPay => TotalPay * (TotalDeductions / 100);
        public int TotalDeductions { get; set; }
        public string Platform { get; set; }
        public DateTime Date { get; set; }
        public bool IsVariablesSet { get; set; }
        public Guid PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }
    }
}