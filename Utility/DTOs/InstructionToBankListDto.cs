using System.Collections.Generic;

namespace Utility.DTOs
{
    public class InstructionToBankListDto
    {
        public string Title { get; set; }
        public string Vessel { get; set; }
        public string Date { get; set; }
        public int PersonnelCount { get; set; }
        public string GrandTotal { get; set; }
    }
}
