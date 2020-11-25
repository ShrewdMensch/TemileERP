using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility.DTOs;

namespace Web.API
{
    public class VesselsController : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<PaymentSlipDto>> GetAll()
        {
            var vessels = await Repository.GetAll<Vessel>();

            return Ok(vessels);
        }

        [HttpGet("ForSelect2")]
        public async Task<ActionResult<PaymentSlipDto>> GetAllForDropDown()
        {
            var vessels = await Repository.GetAll<Vessel>();

            return Ok(Mapper.Map<IEnumerable<Vessel>, IEnumerable<Select2InputDto>>(vessels));
        }
    }
}