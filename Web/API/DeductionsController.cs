using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility.DTOs;

namespace Web.API
{
    public class DeductionsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<DeductionDto>> Get(Guid id)
        {
            var deduction = await Repository.Get<Deduction>(id);
            return Ok(Mapper.Map<Deduction, DeductionDto>(deduction));
        }

        [HttpGet("All")]
        public async Task<ActionResult<DeductionDto>> GetAll()
        {
            var deductions = await Repository.GetAll<Deduction>();

            return Ok(Mapper.Map<IEnumerable<Deduction>, IEnumerable<DeductionDto>>(deductions));
        }


    }
}