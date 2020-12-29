using System;
using Domain.Utility;

namespace Utility.InputModels
{
    public class PersonnelInputModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string OtherName { get; set; }
        public Sex Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Designation { get; set; }
        public string DateJoined { get; set; }
        public Double DailyRate { get; set; }
        public string Bank { get; set; }
        public string Vessel { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PhoneNo { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNo { get; set; }
    }
}