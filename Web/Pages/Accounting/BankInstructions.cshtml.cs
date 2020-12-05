using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Pages;
using Domain;
using Utility;
using Utility.DTOs;
using Utility.InputModels;
using Microsoft.EntityFrameworkCore;
using static Utility.UtilityClasses;
using System.Linq;

namespace Web.Pages.Accounting
{
    public class BankInstructionsModel : BasePageModel
    {
        private readonly ILogger<BankInstructionsModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public BankInstructionsModel(ILogger<BankInstructionsModel> logger, IRepository repository, IMapper mapper, IUserAccessor userAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }



        [BindProperty]
        public IEnumerable<InstructionToBankListDto> BankInstructions { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var payrolls = await _repository.GetCurrentPayrolls();

            var bankInstructions = payrolls.GroupBy(p=>p.Vessel, (vessel, payroll)=> new InstructionToBankListDto
            {
                Title = String.Format("Instructions To Bank For {0} Vessel", vessel),
                Vessel = vessel,
                Date = payroll.FirstOrDefault()?.Date.ToFormalMonthAndYear(),
                PersonnelCount = payroll.Count(),
                GrandTotal = payroll.Sum(p=>p.NetPay).ToCurrency()
            });

            BankInstructions = bankInstructions.DistinctByVessel();

            return Page();
        }
    }
}