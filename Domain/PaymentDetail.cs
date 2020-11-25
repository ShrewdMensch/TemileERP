using System;

namespace Domain
{
    public class PaymentDetail
    {
        public Guid Id { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PayrollId { get; set; }
        public virtual Payroll Payroll { get; set; }
    }
}