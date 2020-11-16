using System;

namespace Domain
{
    public class DeductionSummary
    {
        public Guid Id { get; set; }
        public string DeductionName { get; set; }
        public float DeductionPercentage { get; set; }
        public Guid PayrollId { get; set; }
        public virtual Payroll Payroll { get; set; }
    }
}