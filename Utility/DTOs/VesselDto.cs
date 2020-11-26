using System;

namespace Utility.DTOs
{
    public class VesselDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DateAdded { get; set; }
        public string LastModified { get; set; }
    }
}
