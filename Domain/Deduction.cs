using System;

namespace Domain
{
    public class Deduction
    {
        public Deduction()
        {
            DateAdded = DateTime.Now;
            LastModified = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Percentage { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastModified { get; set; }
        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser ModifiedBy { get; set; }
    }
}