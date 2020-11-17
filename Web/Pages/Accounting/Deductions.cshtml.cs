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

namespace Web.Pages.Accounting
{
    public class DeductionsModel : BasePageModel
    {
        private readonly ILogger<DeductionsModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public DeductionsModel(ILogger<DeductionsModel> logger, IRepository repository, IMapper mapper, IUserAccessor userAccessor)
        {
            _logger = logger;
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
                MessageIcon = MessageType.Success;
                MessageBody = "Deduction has been added successfully";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Deduction could not be added!";
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
                MessageIcon = MessageType.Error;
                MessageBody = "Deduction could not be updated!";

                return RedirectToPage();
            }

            deduction.LastModified = DateTime.Now;
            deduction.ModifiedBy = currentUser;
            deduction.Name = deductionInput.Name.ToTitleCase();
            deduction.Percentage = deductionInput.Percentage;


            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Deduction has been updated successfully";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Deduction could not be updated!";
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteDeductionAsync(DeductionInputModel deductionInput)
        {
            MessageTitle = "Deduction Deletion";
            var currentUser = _userAccessor.GetCurrentUser();
            var deduction = await _repository.Get<Deduction>(deductionInput.Id);

            if (deduction == null)
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Deduction could not be deleted!";

                return RedirectToPage();
            }

            _repository.Remove<Deduction>(deduction);

            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Deduction has been deleted successfully";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Deduction could not be deleted!";
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
                currentPayroll.PaymentDetail.BVN = currentPayroll.Personnel.BVN;
                currentPayroll.PaymentDetail.AccountName = currentPayroll.Personnel.AccountName;
                currentPayroll.PaymentDetail.AccountNumber = currentPayroll.Personnel.AccountNumber;
            }

            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Deduction has been successfully applied to all current payroll";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Deduction could not be applied to all current payroll!";
            }

            return RedirectToPage("./CurrentVariables");
        }

    }
}