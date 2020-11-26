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
        [HttpGet("{id}")]
        public async Task<ActionResult<VesselDto>> Get(Guid id)
        {
            var vessel = await Repository.Get<Vessel>(id);

            return Ok(Mapper.Map<Vessel,VesselDto>(vessel));
        }
        
        [HttpGet("All")]
        public async Task<ActionResult<VesselDto>> GetAll()
        {
            var vessels = await Repository.GetAll<Vessel>();

            return Ok(vessels);
        }

        [HttpGet("ForSelect2")]
        public async Task<ActionResult<IEnumerable<Select2InputDto>>> GetAllForDropDown()
        {
            var vessels = await Repository.GetAll<Vessel>();

            return Ok(Mapper.Map<IEnumerable<Vessel>, IEnumerable<Select2InputDto>>(vessels));
        }
    }
}