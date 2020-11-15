using System;

namespace Utility.DTOs
{
    public class PersonnelDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OtherName { get; set; }

        public string MiddleInitial { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public Double DailyRate { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BVN { get; set; }
        public string PhoneNumber { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public string Photo { get; set; }
    }
}