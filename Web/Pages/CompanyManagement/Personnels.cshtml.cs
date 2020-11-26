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

namespace Web.Pages.CompanyManagement
{
    public class PersonnelsModel : BasePageModel
    {
        private readonly ILogger<PersonnelsModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public PersonnelsModel(ILogger<PersonnelsModel> logger, IRepository repository, IMapper mapper)
        {
            _logger = logger;
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
            var otherNameIsEmptyOrNull = String.IsNullOrWhiteSpace(personnelInputModel.OtherName);
            MessageTitle = "Personnel Creation";

            var personnel = new Personnel
            {
                FirstName = personnelInputModel.FirstName.ToTitleCase(),
                LastName = personnelInputModel.Surname.ToTitleCase(),
                OtherName = otherNameIsEmptyOrNull ? null : personnelInputModel.OtherName.ToTitleCase(),
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
                Address = personnelInputModel.Address
            };

            personnel.Id = await _repository.GenerateNewPersonnelId(personnel);

            _repository.Add(personnel);

            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Personnel has been added successfully";
            }
            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Personnel could not be added";
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditPersonnelAsync(PersonnelInputModel personnelInputModel)
        {
            var otherNameIsEmptyOrNull = String.IsNullOrWhiteSpace(personnelInputModel.OtherName);

            var personnel = new Personnel
            {
                Id = personnelInputModel.Id,
                FirstName = personnelInputModel.FirstName.ToTitleCase(),
                LastName = personnelInputModel.Surname.ToTitleCase(),
                OtherName = otherNameIsEmptyOrNull ? null : personnelInputModel.OtherName.ToTitleCase(),
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
                Address = personnelInputModel.Address
            };

            _repository.Attach<Personnel>(personnel).State = EntityState.Modified;

            try
            {
                await _repository.SaveAll();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelExists(personnel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            MessageTitle = "Personnel Information Update";
            MessageIcon = MessageType.Success;
            MessageBody = "Personnel has been updated successfully";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetEnableOrDisablePersonnel(string id)
        {
            var personnel = await _repository.Get<Personnel>(id);

            personnel.IsActive = !personnel.IsActive;

            var clause = (personnel.IsActive) ? "active" : "inactive";

            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = String.Format("Personnel, {1} has been successfully set {0}", clause, personnel.Name);
            }
            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = String.Format("Personnel {1} could not be set {0}", clause, personnel.Name);
            }
            return RedirectToPage();
        }

        private bool PersonnelExists(string id)
        {
            return _repository.Get<Personnel>(id) != null;
        }
    }
}