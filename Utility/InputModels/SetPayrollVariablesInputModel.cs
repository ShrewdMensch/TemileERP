using System;

namespace Utility.InputModels
{
    public class SetPayrollVariablesInputModel
    {
        public string PersonnelId { get; set; }
        public string PayrollId { get; set; }
        public string PersonnelName { get; set; }
        public double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public string Vessel { get; set; }
    }
}