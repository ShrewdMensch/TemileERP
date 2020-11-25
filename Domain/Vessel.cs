using Domain.Utility;
using System;

namespace Domain
{
    public class Vessel: History
    {
        public Vessel()
        {
            DateAdded = DateTime.Now;
            LastModified = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
       
    }
}
