using System;

namespace Domain
{
    public class Allowance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string PayrollId { get; set; }
        public virtual Payroll Payroll { get; set; }
    }
}
