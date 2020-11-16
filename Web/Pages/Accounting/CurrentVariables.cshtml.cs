using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utility;
using Domain;
using Utility.DTOs;
using Utility.InputModels;
using static Utility.UtilityClasses;

namespace Web.Pages.Accounting
{
    public class CurrentVariablesModel : BasePageModel
    {
        private readonly ILogger<CurrentVariablesModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CurrentVariablesModel(ILogger<CurrentVariablesModel> logger, IRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [BindProperty]
        public IEnumerable<PersonnelPayrollDto> PersonnelCurrentPayrolls { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var personnels = await _repository.GetAll<Personnel>();

            PersonnelCurrentPayrolls = _mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelPayrollDto>>(personnels);

            return Page();
        }

        public async Task<IActionResult> OnPostSetCurrentPayrollVariablesAsync(SetPayrollVariablesInputModel setPayrollVariablesInput)
        {
            var personnel = await _repository.Get<Personnel>(setPayrollVariablesInput.PersonnelId);
            var payroll = await _repository.Get<Payroll>(setPayrollVariablesInput.PayrollId);
            var deductions = await _repository.GetAll<Deduction>();

            if (personnel == null)
            {
                MessageTitle = "Payroll Variables Update";
                MessageIcon = MessageType.Error;
                MessageBody = "Personnel's payroll variables could not be updated";

                return RedirectToPage();
            }

            if (payroll == null) //No current Payroll yet - Create new one
            {
                var newPayroll = new Payroll
                {
                    DaysWorked = setPayrollVariablesInput.DaysWorked,
                    DailyRate = setPayrollVariablesInput.DailyRate,
                    Platform = setPayrollVariablesInput.Platform,
                    PersonnelId = setPayrollVariablesInput.PersonnelId,
                    TotalDeductions = await _repository.TotalDeduction()
                };

                _repository.Add<Payroll>(newPayroll);


                foreach (var deduction in deductions)
                {
                    var deductionSummary = new DeductionSummary
                    {
                        DeductionName = deduction.Name,
                        DeductionPercentage = deduction.Percentage,
                        Payroll = newPayroll
                    };

                    _repository.Add(deductionSummary);
                }

            }

            else
            {
                payroll.DaysWorked = setPayrollVariablesInput.DaysWorked;
                payroll.DailyRate = setPayrollVariablesInput.DailyRate;
                payroll.Platform = setPayrollVariablesInput.Platform;
                payroll.TotalDeductions = await _repository.TotalDeduction();

                //Remove existing deduction summaries before adding possibly updated ones
                _repository.RemoveRange<DeductionSummary>(payroll.DeductionSummaries);

                foreach (var deduction in deductions)
                {
                    var deductionSummary = new DeductionSummary
                    {
                        DeductionName = deduction.Name,
                        DeductionPercentage = deduction.Percentage
                    };

                    payroll.DeductionSummaries.Add(deductionSummary);
                }

            }

            personnel.DailyRate = setPayrollVariablesInput.DailyRate;

            if (await _repository.SaveAll())
            {
                MessageTitle = "Payroll Variables Update";
                MessageIcon = MessageType.Success;
                MessageBody = "Personnel's payroll variables has been updated successfully";
            }

            else
            {
                MessageTitle = "Payroll Variables Update";
                MessageIcon = MessageType.Error;
                MessageBody = "Personnel's payroll variables could not be updated";
            }


            return RedirectToPage();
        }
    }
}
