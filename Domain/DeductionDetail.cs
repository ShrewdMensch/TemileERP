using System;

namespace Domain
{
    public class DeductionDetail
    {
        public Guid Id { get; set; }
        public string DeductionName { get; set; }
        public float DeductedPercentage { get; set; }
        public double DeductedAmount { get; set; }
        public string PayrollId { get; set; }
        public virtual Payroll Payroll { get; set; }
    }
}