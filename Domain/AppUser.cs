using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Domain.Utility;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            DateOfRegistration = DateTime.Now;
            RecommendedToChangePassword = true;
        }

        public string StaffId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Other Name")]
        public string OtherName { get; set; }

        public string MiddleInitial => OtherName.GetInitial();

        public string Name => FirstName.Combine(MiddleInitial, LastName);

        public Sex? Sex { get; set; }

        public DateTime? DOB { get; set; }

        public string Designation { get; set; }

        public string Address { get; set; }

        [Display(Name = "Active ?")]
        public bool Locked => !(LockoutEnd == null || LockoutEnd < DateTime.Now);

        public DateTime? LastSeen { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public bool RecommendedToChangePassword { get; set; }

        public virtual Photo Photo { get; set; }

    }
}