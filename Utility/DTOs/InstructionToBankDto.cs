using System.Collections.Generic;

namespace Utility.DTOs
{
    public class InstructionToBankDto
    {
        public string Vessel { get; set; }
        public string Date { get; set; }
        public List<InstructionToBankDetailsDto> Details { get; set; }
        public string GrandTotal { get; set; }
    }
}
