using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility.DTOs;
using Utility;

namespace Web.API
{
    public class BankInstructionsController : BaseController
    {
        [HttpGet("{vessel}")]
        public async Task<ActionResult<IEnumerable<InstructionToBankDto>>> GetAllByVessel(string vessel)
        {
            var payrolls = await Repository.GetCurrentPayrollsByVessel(vessel);

            var instructionToBank = new InstructionToBankDto { 
                Vessel = payrolls.FirstOrDefault()?.Vessel,
                Date = payrolls.FirstOrDefault()?.Date.ToFormalMonthAndYear(),
                Details = Mapper.Map<IEnumerable<Payroll>, List<InstructionToBankDetailsDto>>(payrolls),
                GrandTotal = payrolls.Sum(p=>p.NetPay).ToCurrency()
            };

            return Ok(instructionToBank);
        }
    }
}