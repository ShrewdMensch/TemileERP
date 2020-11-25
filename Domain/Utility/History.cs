using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Utility
{
    public class History
    {
        public DateTime DateAdded { get; set; }
        public DateTime LastModified { get; set; }
        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser ModifiedBy { get; set; }
    }
}
