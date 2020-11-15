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
            var myTextInfo = new CultureInfo("en-US", false).TextInfo;
            var otherNameIsEmptyOrNull = String.IsNullOrWhiteSpace(personnelInputModel.OtherName);

            var personnel = new Personnel
            {
                FirstName = myTextInfo.ToTitleCase(personnelInputModel.FirstName),
                LastName = myTextInfo.ToTitleCase(personnelInputModel.Surname),
                OtherName = otherNameIsEmptyOrNull ? null : myTextInfo.ToTitleCase(personnelInputModel.OtherName),
                Sex = personnelInputModel.Sex,
                Nationality = personnelInputModel.Nationality,
                Religion = personnelInputModel.Religion,
                Email = personnelInputModel.Email,
                DailyRate = personnelInputModel.DailyRate,
                Bank = personnelInputModel.Bank,
                AccountName = personnelInputModel.AccountName,
                AccountNumber = personnelInputModel.AccountNumber,
                BVN = personnelInputModel.BVN,
                PhoneNumber = personnelInputModel.PhoneNo,
                NextOfKin = personnelInputModel.NextOfKin,
                NextOfKinPhoneNumber = personnelInputModel.NextOfKinPhoneNo,
                Address = personnelInputModel.Address
            };

            _repository.Add<Personnel>(personnel);

            if (await _repository.SaveAll())
            {
                MessageTitle = "Personnel Information Update";
                MessageIcon = MessageType.Success;
                MessageBody = "Personnel has been updated successfully";
            }
            else
            {
                MessageTitle = "Personnel Information Update";
                MessageIcon = MessageType.Error;
                MessageBody = "Personnel could not be updated";
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditPersonnelAsync(PersonnelInputModel personnelInputModel)
        {

            var myTextInfo = new CultureInfo("en-US", false).TextInfo;
            var otherNameIsEmptyOrNull = String.IsNullOrWhiteSpace(personnelInputModel.OtherName);

            var personnel = new Personnel
            {
                Id = personnelInputModel.Id,
                FirstName = myTextInfo.ToTitleCase(personnelInputModel.FirstName),
                LastName = myTextInfo.ToTitleCase(personnelInputModel.Surname),
                OtherName = otherNameIsEmptyOrNull ? null : myTextInfo.ToTitleCase(personnelInputModel.OtherName),
                Sex = personnelInputModel.Sex,
                Nationality = personnelInputModel.Nationality,
                Religion = personnelInputModel.Religion,
                Email = personnelInputModel.Email,
                DailyRate = personnelInputModel.DailyRate,
                Bank = personnelInputModel.Bank,
                AccountName = personnelInputModel.AccountName,
                AccountNumber = personnelInputModel.AccountNumber,
                BVN = personnelInputModel.BVN,
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
        private bool PersonnelExists(Guid id)
        {
            return _repository.Get<Personnel>(id) != null;
        }
    }
}