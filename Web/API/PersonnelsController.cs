using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility;
using Utility.DTOs;

namespace Web.API
{
    public class PersonnelsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonnelDto>> Get(string id)
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

        [HttpGet("AllPlusPayroll")]
        public async Task<ActionResult<PersonnelDto>> GetAllPlusPayrollDetails()
        {
            var personnels = await Repository.GetAll<Personnel>();

            return Ok(Mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelPayrollDto>>(personnels));
        }

        [HttpGet("HasCurrentPayroll")]
        public async Task<ActionResult<PersonnelPayrollDto>> GetPersonnelsWithCurrentPayroll()
        {
            var personnels = await Repository.GetPersonnelsWithCurrentPayroll();

            return Ok(Mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelPayrollDto>>(personnels));
        }

        [HttpGet("HasNoCurrentPayroll")]
        public async Task<ActionResult<PersonnelPayrollDto>> GetPersonnelsWithoutCurrentPayroll()
        {
            var personnelsWithCurrentPayroll = await Repository.GetPersonnelsWithCurrentPayroll();

            var personnelsWithoutCurrentPayroll = (await Repository.GetAll<Personnel>()).Except(personnelsWithCurrentPayroll);

            return Ok(Mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelPayrollDto>>(personnelsWithoutCurrentPayroll));
        }

        [HttpGet("{id}/CurrentPayroll")]
        public async Task<ActionResult<PersonnelPayrollDto>> GetCurrentPayroll(string id)
        {
            var personnel = await Repository.Get<Personnel>(id);

            var currentPayroll = personnel.Payrolls.GetCurrentPayroll();

            return Ok(Mapper.Map<Payroll, PersonnelPayrollDto>(currentPayroll));
        }

    }
}