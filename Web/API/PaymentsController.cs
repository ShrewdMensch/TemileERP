using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility.DTOs;

namespace Web.API
{
    public class PaymentsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentSlipDto>> Get(string id)
        {
            var payment = await Repository.Get<Payroll>(id);
            return Ok(Mapper.Map<Payroll, PaymentSlipDto>(payment));
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<PaymentSlipDto>>> GetAll()
        {
            var payments = await Repository.GetAll<Payroll>();

            return Ok(Mapper.Map<IEnumerable<Payroll>, IEnumerable<PaymentSlipDto>>(payments));
        }

    }
}