using System;

namespace Utility.InputModels
{
    public class SetPayrollVariablesInputModel
    {
        public string PersonnelId { get; set; }
        public string PayrollId { get; set; }
        public string PersonnelName { get; set; }
        public double DailyRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool WorkedWeekend { get; set; }
        public string Vessel { get; set; }
    }
}