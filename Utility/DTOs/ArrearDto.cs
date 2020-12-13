using System;

namespace Utility.DTOs
{
    public class ArrearDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Period { get; set; }
        public string PersonnelDailyRate { get; set; }
        public string PersonnelDesignation { get; set; }
        public string Vessel { get; set; }
        public string DaysWorked { get; set; }
        public bool WorkedWeekend { get; set; }
        public string Amount { get; set; }
    }
}