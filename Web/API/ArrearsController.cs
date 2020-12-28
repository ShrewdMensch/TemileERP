using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace Web.API
{
    public class ArrearsController : BaseController
    {
        [HttpGet("validate")]
        public async Task<ActionResult> IsArrearsPeriodValid(string period, string personnelId)
        {
            var dateRange = period.ToDateRange();

            var payroll = await Repository.GetPersonnelPayrollByMonth(personnelId, dateRange.StartDate);

            if (dateRange.StartDate.HasSameMonthAndYearWith(dateRange.EndDate) && payroll != null)
            {
                return Ok();
            }

            else
            {
                return NotFound();

            }
        }
    }

}