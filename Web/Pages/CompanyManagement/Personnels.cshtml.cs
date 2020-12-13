using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Utility;
using Utility.DTOs;
using Utility.InputModels;
using Microsoft.EntityFrameworkCore;
using static Utility.UtilityClasses;

namespace Web.Pages.CompanyManagement
{
    public class PersonnelsModel : BasePageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public PersonnelsModel(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [BindProperty]
        public IEnumerable<PersonnelDto> Personnels { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var personnels = await _repository.GetAll<Personnel>();
            Personnels = _mapper.Map<IEnumerable<Personnel>, IEnumerable<PersonnelDto>>(personnels);

            return Page();
        }
        public async Task<IActionResult> OnPostCreatePersonnelAsync(PersonnelInputModel personnelInputModel)
        {
            MessageTitle = "Personnel Creation";

            var personnel = new Personnel
            {
                FirstName = personnelInputModel.FirstName.ToTitleCase(),
                LastName = personnelInputModel.Surname.ToTitleCase(),
                OtherName = personnelInputModel.OtherName.ToTitleCase(),
                Sex = personnelInputModel.Sex,
                Nationality = personnelInputModel.Nationality,
                Religion = personnelInputModel.Religion,
                Email = personnelInputModel.Email,
                DailyRate = personnelInputModel.DailyRate,
                Bank = personnelInputModel.Bank,
                Vessel = personnelInputModel.Vessel,
                AccountName = personnelInputModel.AccountName,
                AccountNumber = personnelInputModel.AccountNumber,
                PhoneNumber = personnelInputModel.PhoneNo,
                NextOfKin = personnelInputModel.NextOfKin,
                NextOfKinPhoneNumber = personnelInputModel.NextOfKinPhoneNo,
                Address = personnelInputModel.Address,
                Designation = personnelInputModel.Designation
            };

            personnel.Id = await _repository.GenerateNewPersonnelId(personnel);

            _repository.Add(personnel);

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Personnel has been added successfully", MessageType.Success);
            }
            else
            {
                SetNotificationMessageAndIcon("Personnel could not be added", MessageType.Error);
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditPersonnelAsync(PersonnelInputModel personnelInputModel)
        {
            MessageTitle = "Personnel Information Update";

            var personnelInDb = await _repository.Get<Personnel>(personnelInputModel.Id);

            if (personnelInDb == null)
            {
                SetNotificationMessageAndIcon("Personnel could not be updated", MessageType.Error);
            }

            else
            {
                UpdatePersonnelInDataBase(personnelInputModel, personnelInDb);

                if (await _repository.SaveAll())
                {
                    SetNotificationMessageAndIcon("Personnel has been updated successfully", MessageType.Success);
                }
                else
                {
                    SetNotificationMessageAndIcon("Personnel could not be updated", MessageType.Error);
                }

            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetEnableOrDisablePersonnel(string id)
        {
            var personnel = await _repository.Get<Personnel>(id);

            MessageTitle = "Personnel Status Update";

            personnel.IsActive = !personnel.IsActive;

            var clause = (personnel.IsActive) ? "active" : "inactive";

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon(string.Format("Personnel, {1} has been successfully set {0}",
                    clause, personnel.Name), MessageType.Success);
            }
            else
            {
                SetNotificationMessageAndIcon(string.Format("Personnel {1} could not be set {0}",
                    clause, personnel.Name), MessageType.Error);
            }
            return RedirectToPage();
        }

        private void UpdatePersonnelInDataBase(PersonnelInputModel personnelInputModel, Personnel personnelInDb)
        {
            personnelInDb.Id = personnelInputModel.Id;
            personnelInDb.FirstName = personnelInputModel.FirstName.ToTitleCase();
            personnelInDb.LastName = personnelInputModel.Surname.ToTitleCase();
            personnelInDb.OtherName = personnelInputModel.OtherName.ToTitleCase();
            personnelInDb.Sex = personnelInputModel.Sex;
            personnelInDb.Nationality = personnelInputModel.Nationality;
            personnelInDb.Religion = personnelInputModel.Religion;
            personnelInDb.Email = personnelInputModel.Email;
            personnelInDb.DailyRate = personnelInputModel.DailyRate;
            personnelInDb.Bank = personnelInputModel.Bank;
            personnelInDb.Vessel = personnelInputModel.Vessel;
            personnelInDb.AccountName = personnelInputModel.AccountName;
            personnelInDb.AccountNumber = personnelInputModel.AccountNumber;
            personnelInDb.PhoneNumber = personnelInputModel.PhoneNo;
            personnelInDb.NextOfKin = personnelInputModel.NextOfKin;
            personnelInDb.NextOfKinPhoneNumber = personnelInputModel.NextOfKinPhoneNo;
            personnelInDb.Address = personnelInputModel.Address;
            personnelInDb.Designation = personnelInputModel.Designation;

            /*var currentPayroll = personnelInDb.Payrolls.GetCurrentPayroll();

            if (currentPayroll != null)
            {
                currentPayroll.Vessel = personnelInputModel.Vessel;
                currentPayroll.DailyRate = personnelInputModel.DailyRate;
                currentPayroll.PersonnelDesignation = personnelInputModel.Designation;

                await _repository.ReApplyVariablesOnCurrentPayroll(currentPayroll);

            }*/
        }
    }
}