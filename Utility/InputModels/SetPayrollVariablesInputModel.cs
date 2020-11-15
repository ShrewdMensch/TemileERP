using System;

namespace Utility.InputModels
{
    public class SetPayrollVariablesInputModel
    {
        public Guid PersonnelId { get; set; }
        public Guid PayrollId { get; set; }
        public string PersonnelName { get; set; }
        public double DailyRate { get; set; }
        public int DaysWorked { get; set; }
        public string Platform { get; set; }
    }
}