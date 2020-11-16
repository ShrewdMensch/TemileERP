using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility.DTOs;

namespace Web.API
{
    public class PersonnelsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelDto>> Get(Guid id)
        {
            var user = await Repository.Get<Personnel>(id);
            return Ok(Mapper.Map<Personnel, PersonnelDto>(user));
        }

        [HttpGet("All")]
        public async Task<ActionResult<PersonnelDto>> GetAll()
        {
            var personnels = await Repository.GetAll<Personnel>();

            return Ok(Mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelDto>>(personnels));
        }

        [HttpGet("{id}/CurrentPayroll")]
        public async Task<ActionResult<PersonnelPayrollDto>> GetCurrentPayroll(Guid id)
        {
            var personnel = await Repository.Get<Personnel>(id);

            return Ok(Mapper.Map<Personnel, PersonnelPayrollDto>(personnel));
        }

    }
}