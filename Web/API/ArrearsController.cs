using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility;
using Utility.DTOs;

namespace Web.API
{
    public class ArrearsController : BaseController
    {
        [HttpGet("validate")]
        public async Task<ActionResult<bool>> IsArrearsPeriodValid(string period, string personnelId)
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