using System;

namespace Utility.DTOs
{
    public class InstructionToBankListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Vessel { get; set; }
        public string Date { get; set; }
        public int PersonnelCount { get; set; }
        public string GrandTotal { get; set; }
        public int SentCount { get; set; }
    }
}
