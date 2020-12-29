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
using System.Linq;

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
            var personnels = await _repository.GetAllPersonnelsOrderByStatus();

            PersonnelCurrentPayrolls = _mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelPayrollDto>>(personnels);

            return Page();
        }

        public async Task<IActionResult> OnPostSetCurrentPayrollVariablesAsync(SetPayrollVariablesInputModel setPayrollVariablesInput)
        {
            var personnel = await _repository.Get<Personnel>(setPayrollVariablesInput.PersonnelId);
            var payroll = await _repository.Get<Payroll>(setPayrollVariablesInput.PayrollId);
            var deductions = await _repository.GetAll<Deduction>();

            MessageTitle = "Payroll Variables Update";

            if (personnel == null)
            {
                SetNotificationMessageAndIcon("Personnel's payroll variables could not be updated", MessageType.Error);
                return RedirectToPage();
            }

            personnel.Vessel = setPayrollVariablesInput.Vessel;
            personnel.DailyRate = setPayrollVariablesInput.DailyRate;

            if (payroll == null) //No current Payroll yet - Create new one
            {
                await CreatePayroll(setPayrollVariablesInput, deductions);
            }

            else
            {
                await UpdatePayroll(setPayrollVariablesInput, payroll, deductions);

            }

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Personnel's payroll variables has been updated successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Personnel's payroll variables could not be updated", MessageType.Error);
            }


            return RedirectToPage();
        }

        private async Task CreatePayroll(SetPayrollVariablesInputModel setPayrollVariablesInput, IEnumerable<Deduction> deductions)
        {
            var newPayroll = new Payroll
            {
                StartDate = setPayrollVariablesInput.StartDate,
                EndDate = setPayrollVariablesInput.EndDate,
                WorkedWeekend = setPayrollVariablesInput.WorkedWeekend,
                DailyRate = setPayrollVariablesInput.DailyRate,
                Vessel = setPayrollVariablesInput.Vessel,
                PersonnelId = setPayrollVariablesInput.PersonnelId,
                TotalDeductedPercentage = await _repository.TotalDeduction()
            };

            newPayroll.Id = await _repository.GenerateNewPayrollId();

            _repository.Add(newPayroll);

            newPayroll.Personnel.Vessel = setPayrollVariablesInput.Vessel;
            newPayroll.PersonnelDesignation = newPayroll.Personnel.Designation;

            AddDeductionDetails(deductions, newPayroll);
            AddPaymentDetail(newPayroll);
            AddAllowances(setPayrollVariablesInput, newPayroll);
            AddSpecificDeductions(setPayrollVariablesInput, newPayroll);
            await AddArrears(setPayrollVariablesInput, newPayroll);
        }


        private async Task UpdatePayroll(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll payroll, IEnumerable<Deduction> deductions)
        {
            payroll.StartDate = setPayrollVariablesInput.StartDate;
            payroll.EndDate = setPayrollVariablesInput.EndDate;
            payroll.WorkedWeekend = setPayrollVariablesInput.WorkedWeekend;
            payroll.DailyRate = setPayrollVariablesInput.DailyRate;
            payroll.Vessel = setPayrollVariablesInput.Vessel;
            payroll.TotalDeductedPercentage = await _repository.TotalDeduction();

            payroll.Personnel.Vessel = setPayrollVariablesInput.Vessel;

            UpdateDeductionDetails(payroll, deductions);
            UpdatePaymentDetail(payroll);
            UpdateAllowances(setPayrollVariablesInput, payroll);
            UpdateSpecificDeductions(setPayrollVariablesInput, payroll);
            await UpdateArrears(setPayrollVariablesInput, payroll);
        }

        private static void UpdatePaymentDetail(Payroll payroll)
        {
            payroll.PaymentDetail.Bank = payroll.Personnel.Bank;
            payroll.PaymentDetail.AccountName = payroll.Personnel.AccountName;
            payroll.PaymentDetail.AccountNumber = payroll.Personnel.AccountNumber;
        }

        private void UpdateSpecificDeductions(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll payroll)
        {
            //Remove existing specific deductions before adding possibly updated ones
            //Case where Updating with no specific deduction i.e remove all existing ones without adding new one(s)
            _repository.RemoveRange(payroll.SpecificDeductions);

            if (setPayrollVariablesInput.SpecificDeductionAmounts == null) return;

            for (var count = 0; count < setPayrollVariablesInput.SpecificDeductionAmounts.Count(); count++)
            {
                var newSpecificDeduction = new SpecificDeduction
                {
                    Name = setPayrollVariablesInput.SpecificDeductionNames[count].ToTitleCase(),
                    Amount = setPayrollVariablesInput.SpecificDeductionAmounts[count]
                };

                payroll.SpecificDeductions.Add(newSpecificDeduction);
            }
        }

        private void UpdateAllowances(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll payroll)
        {
            //Remove existing allowances before adding possibly updated ones
            //Case where Updating with no allowance i.e remove all existing ones without adding new one(s)
            _repository.RemoveRange(payroll.Allowances);

            if (setPayrollVariablesInput.AllowanceAmounts == null) return;

            for (var count = 0; count < setPayrollVariablesInput.AllowanceAmounts.Count(); count++)
            {
                var newAllowance = new Allowance
                {
                    Name = setPayrollVariablesInput.AllowanceNames[count].ToTitleCase(),
                    Amount = setPayrollVariablesInput.AllowanceAmounts[count]
                };

                payroll.Allowances.Add(newAllowance);
            }
        }

        private async Task UpdateArrears(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll payroll)
        {
            //Remove existing allowances before adding possibly updated ones
            //Case where Updating with no allowance i.e remove all existing ones without adding new one(s)
            _repository.RemoveRange(payroll.Arrears);

            if (setPayrollVariablesInput.ArrearPeriods == null) return;

            for (var count = 0; count < setPayrollVariablesInput.ArrearPeriods.Count(); count++)
            {
                var dateRange = setPayrollVariablesInput.ArrearPeriods[count].ToDateRange();

                var foundPayroll = await _repository.GetPersonnelPayrollByMonth(payroll.PersonnelId, dateRange.StartDate);

                if (dateRange.StartDate.HasSameMonthAndYearWith(dateRange.EndDate) && foundPayroll != null)
                {
                    var newArrear = new Arrear
                    {
                        StartDate = dateRange.StartDate,
                        EndDate = dateRange.EndDate,
                        AffectedPayroll = await _repository.GetPersonnelPayrollByMonth(payroll.PersonnelId, dateRange.StartDate),
                        CorrectivePayroll = payroll
                    };

                    _repository.Add(newArrear);

                }


            }
        }

        private void UpdateDeductionDetails(Payroll payroll, IEnumerable<Deduction> deductions)
        {
            //Remove existing deduction details before adding possibly updated ones
            //Case where Updating with no deductionDetail i.e remove all existing ones without adding new one(s)
            _repository.RemoveRange(payroll.DeductionDetails);

            foreach (var deduction in deductions)
            {
                var deductionDetail = new DeductionDetail
                {
                    DeductionName = deduction.Name,
                    DeductedPercentage = deduction.Percentage,
                    DeductedAmount = payroll.GrossPay * (deduction.Percentage / 100)
                };

                payroll.DeductionDetails.Add(deductionDetail);
            }
        }

        private void AddPaymentDetail(Payroll newPayroll)
        {
            var newPayrollPaymentDetail = new PaymentDetail
            {
                Bank = newPayroll.Personnel.Bank,
                AccountName = newPayroll.Personnel.AccountName,
                AccountNumber = newPayroll.Personnel.AccountNumber,
                Payroll = newPayroll
            };

            _repository.Add(newPayrollPaymentDetail);
        }

        private void AddDeductionDetails(IEnumerable<Deduction> deductions, Payroll newPayroll)
        {
            foreach (var deduction in deductions)
            {
                var newDeductionDetail = new DeductionDetail
                {
                    DeductionName = deduction.Name,
                    DeductedPercentage = deduction.Percentage,
                    DeductedAmount = newPayroll.GrossPay * (deduction.Percentage / 100),
                    Payroll = newPayroll
                };

                _repository.Add(newDeductionDetail);
            }
        }

        private void AddSpecificDeductions(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll newPayroll)
        {
            if (setPayrollVariablesInput.SpecificDeductionAmounts == null) return;

            for (var count = 0; count < setPayrollVariablesInput.SpecificDeductionAmounts.Count(); count++)
            {
                var newSpecificDeduction = new SpecificDeduction
                {
                    Name = setPayrollVariablesInput.SpecificDeductionNames[count].ToTitleCase(),
                    Amount = setPayrollVariablesInput.SpecificDeductionAmounts[count],
                    Payroll = newPayroll
                };

                _repository.Add(newSpecificDeduction);
            }
        }

        private void AddAllowances(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll newPayroll)
        {
            if (setPayrollVariablesInput.AllowanceAmounts == null) return;

            for (var count = 0; count < setPayrollVariablesInput.AllowanceAmounts.Count(); count++)
            {
                var newAllowance = new Allowance
                {
                    Name = setPayrollVariablesInput.AllowanceNames[count].ToTitleCase(),
                    Amount = setPayrollVariablesInput.AllowanceAmounts[count],
                    Payroll = newPayroll
                };

                _repository.Add(newAllowance);
            }
        }
        private async Task AddArrears(SetPayrollVariablesInputModel setPayrollVariablesInput, Payroll payroll)
        {
            if (setPayrollVariablesInput.ArrearPeriods == null) return;

            for (var count = 0; count < setPayrollVariablesInput.ArrearPeriods.Count(); count++)
            {
                var dateRange = setPayrollVariablesInput.ArrearPeriods[count].ToDateRange();

                var foundPayroll = await _repository.GetPersonnelPayrollByMonth(payroll.PersonnelId, dateRange.StartDate);

                if (dateRange.StartDate.HasSameMonthAndYearWith(dateRange.EndDate) && foundPayroll != null)
                {
                    var newArrear = new Arrear
                    {
                        StartDate = dateRange.StartDate,
                        EndDate = dateRange.EndDate,
                        AffectedPayroll = await _repository.GetPersonnelPayrollByMonth(payroll.PersonnelId, dateRange.StartDate),
                        CorrectivePayroll = payroll
                    };

                    _repository.Add(newArrear);

                }


            }
        }

        public async Task<IActionResult> OnGetReApplyAsync()
        {
            await _repository.ReApplyVariablesToCurrentPayrolls();

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Payroll Variables have been successfully applied to all current payroll(s)", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Payroll Variables could not be applied to all current payroll(s)!", MessageType.Error);
            }

            return RedirectToPage();
        }
    }
}
