using Domain.Utility;
using System;

namespace Domain
{
    public class Deduction : History
    {
        public Deduction()
        {
            DateAdded = DateTime.Now;
            LastModified = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Percentage { get; set; }
    }
}