using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Utility;
using Utility.DTOs;
using Utility.InputModels;
using static Utility.UtilityClasses;

namespace Web.Pages.Accounting
{
    public class DeductionsModel : BasePageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public DeductionsModel(IRepository repository, IMapper mapper, IUserAccessor userAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        [BindProperty]
        public IEnumerable<DeductionDto> Deductions { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var deductions = await _repository.GetAll<Deduction>();

            Deductions = _mapper.Map<IEnumerable<Deduction>, IEnumerable<DeductionDto>>(deductions);

            return Page();
        }
        public async Task<IActionResult> OnPostCreateDeductionAsync(DeductionInputModel deductionInput)
        {
            MessageTitle = "New Deduction Creation";
            var currentUser = _userAccessor.GetCurrentUser();

            var newDeduction = new Deduction
            {
                Name = deductionInput.Name.ToTitleCase(),
                Percentage = deductionInput.Percentage,
                AddedBy = currentUser,
                ModifiedBy = currentUser
            };

            _repository.Add(newDeduction);

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Deduction has been added successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Deduction could not be added!", MessageType.Error);
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditDeductionAsync(DeductionInputModel deductionInput)
        {
            MessageTitle = "Deduction Update";
            var currentUser = _userAccessor.GetCurrentUser();
            var deduction = await _repository.Get<Deduction>(deductionInput.Id);

            if (deduction == null)
            {
                SetNotificationMessageAndIcon("Deduction could not be updated!", MessageType.Error);

                return RedirectToPage();
            }

            deduction.LastModified = DateTime.Now;
            deduction.ModifiedBy = currentUser;
            deduction.Name = deductionInput.Name.ToTitleCase();
            deduction.Percentage = deductionInput.Percentage;


            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Deduction has been updated successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Deduction could not be updated!", MessageType.Error);
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteDeductionAsync(DeductionInputModel deductionInput)
        {
            MessageTitle = "Deduction Deletion";
            var deduction = await _repository.Get<Deduction>(deductionInput.Id);

            if (deduction == null)
            {
                SetNotificationMessageAndIcon("Deduction could not be deleted!", MessageType.Error);

                return RedirectToPage();
            }

            _repository.Remove(deduction);

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Deduction has been deleted successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Deduction could not be deleted!", MessageType.Error);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetApplyDeductionAsync()
        {
            var currentPayrolls = await _repository.GetCurrentPayrolls();
            var deductions = await _repository.GetAll<Deduction>();
            var totalDeductions = await _repository.TotalDeduction();

            foreach (var currentPayroll in currentPayrolls)
            {
                currentPayroll.TotalDeductedPercentage = totalDeductions;
                _repository.RemoveRange(currentPayroll.DeductionDetails);

                foreach (var deduction in deductions)
                {
                    var deductionDetail = new DeductionDetail
                    {
                        DeductionName = deduction.Name,
                        DeductedPercentage = deduction.Percentage,
                        DeductedAmount = currentPayroll.GrossPay * (deduction.Percentage / 100)
                    };

                    currentPayroll.DeductionDetails.Add(deductionDetail);
                }

                currentPayroll.PaymentDetail.Bank = currentPayroll.Personnel.Bank;
                currentPayroll.PaymentDetail.AccountName = currentPayroll.Personnel.AccountName;
                currentPayroll.PaymentDetail.AccountNumber = currentPayroll.Personnel.AccountNumber;
            }

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Deduction has been successfully applied to all current payroll", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Deduction could not be applied to all current payroll!", MessageType.Error);
            }

            return RedirectToPage("./CurrentVariables");
        }

    }
}