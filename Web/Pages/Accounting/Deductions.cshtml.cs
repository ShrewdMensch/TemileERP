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

        public async Task<IActionResult> OnGetReApplyAsync()
        {
            await _repository.ReApplyVariablesToCurrentPayrolls();

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Deduction(s) have been successfully applied to all current payrolls", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Deduction(s) could not be applied to all current payrolls!", MessageType.Error);
            }

            return RedirectToPage();
        }
    }
}