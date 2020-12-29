using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility;
using static Utility.UtilityFunctions;

namespace Web.API
{
    public class ArrearsController : BaseController
    {
        [HttpGet("validate")]
        public async Task<ActionResult> IsArrearsPeriodValid(string period, string personnelId)
        {
            var dateRange = period.ToDateRange();
            var payroll = await Repository.GetPersonnelPayrollByMonth(personnelId, dateRange.StartDate);
            var personnel = await Repository.Get<Personnel>(personnelId);
            var dateRangeIsValid = IsArrearsDateRangeApplicable(dateRange, payroll, personnel);

            return dateRangeIsValid ? Ok() : (ActionResult)NotFound();
        }
    }

}