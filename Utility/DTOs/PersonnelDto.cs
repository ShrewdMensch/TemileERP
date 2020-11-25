using System;

namespace Utility.DTOs
{
    public class PersonnelDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string MiddleInitial { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Designation { get; set; }
        public string DateJoined { get; set; }
        public double DailyRate { get; set; }
        public string DailyRateStr { get; set; }
        public string Bank { get; set; }
        public string Vessel { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string Photo { get; set; }
    }
}