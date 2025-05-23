using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Utility;

namespace Domain
{
    public class Personnel
    {
        public Personnel()
        {
            IsActive = true;
            DateRegistered = DateTime.Now;
        }
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Other Name")]
        public string OtherName { get; set; }

        public string MiddleInitial => OtherName.GetInitial();

        public string Name => FirstName.Combine(MiddleInitial, LastName);

        public string FullName => FirstName.Combine(OtherName, LastName);

        public Sex Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public Double DailyRate { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public string Designation { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime? DateLeft { get; set; }
        public string Vessel { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }

    }
}