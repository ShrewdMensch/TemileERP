using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility.Extensions;
using static Utility.UtilityFunctions;
using Utility.DTOs;

namespace Web.API
{
    public class PayrollsController : BaseController
    {
        [HttpGet("AllWithPersonnel")]
        public async Task<ActionResult<IEnumerable<PersonnelPayrollDto>>> GetPersonnelPayrolls(string startDate = null, string endDate = null,
             string vessel = null)
        {
            var payrolls = await Repository.GetAll<Payroll>();


            if (startDate != null && endDate != null)
            {
                var parsedStartDate = DateTime.Parse(startDate).Date;
                var parsedEndDate = DateTime.Parse(endDate).Date;

                payrolls = payrolls.Where(p => p.StartDate.Date >= parsedStartDate && p.EndDate.Date <= parsedEndDate);
            }

            if (vessel != null)
            {
                payrolls = payrolls.Where(p => p.Vessel.ToLower() == vessel.ToLower());
            }

            return Ok(Mapper.Map<IEnumerable<Payroll>, IEnumerable<PersonnelPayrollDto>>(payrolls));
        }

        [HttpGet("ValidateDaysWorked")]
        public async Task<ActionResult> IsDaysWorkedValid(string period, string personnelId)
        {
            var dateRange = period.ToDateRange();
            var personnel = await Repository.Get<Personnel>(personnelId);
            var isPeriodValid = IsDaysWorkedApplicableToPersonnel(dateRange, personnel) && 
                !IsDateRangeCapturedByAnyPersonnelPastPayroll(dateRange,personnel);

            return isPeriodValid ? Ok() : (ActionResult)NotFound();
        }

    }
}