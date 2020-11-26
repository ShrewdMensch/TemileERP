using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain;
using Utility;
using Utility.DTOs;
using Utility.InputModels;
using static Utility.UtilityClasses;

namespace Web.Pages.CompanyManagement
{
    public class VesselsModel : BasePageModel
    {
        private readonly ILogger<VesselsModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public VesselsModel(ILogger<VesselsModel> logger, IRepository repository, IMapper mapper, IUserAccessor userAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        [BindProperty]
        public IEnumerable<VesselDto> Vessels { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var vessels = await _repository.GetAll<Vessel>();

            Vessels = _mapper.Map<IEnumerable<Vessel>, IEnumerable<VesselDto>>(vessels);

            return Page();
        }
        public async Task<IActionResult> OnPostCreateVesselAsync(DeductionInputModel deductionInput)
        {
            MessageTitle = "New Vessel Creation";
            var currentUser = _userAccessor.GetCurrentUser();

            var newVessel = new Vessel
            {
                Name = deductionInput.Name.ToTitleCase(),
                AddedBy = currentUser,
                ModifiedBy = currentUser
            };

            _repository.Add(newVessel);

            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Vessel has been added successfully";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Vessel could not be added!";
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditVesselAsync(VesselInputModel vesselInput)
        {
            MessageTitle = "Vessel Update";
            var currentUser = _userAccessor.GetCurrentUser();
            var vessel = await _repository.Get<Vessel>(vesselInput.Id);

            if (vessel == null)
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Vessel could not be updated!";

                return RedirectToPage();
            }

            vessel.LastModified = DateTime.Now;
            vessel.ModifiedBy = currentUser;
            vessel.Name = vesselInput.Name.ToTitleCase();


            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Vessel has been updated successfully";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Vessel could not be updated!";
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteVesselAsync(VesselInputModel vesselInput)
        {
            MessageTitle = "Vessel Deletion";
            var vessel = await _repository.Get<Vessel>(vesselInput.Id);

            if (vessel == null)
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Vessel could not be deleted!";

                return RedirectToPage();
            }

            _repository.Remove(vessel);

            if (await _repository.SaveAll())
            {
                MessageIcon = MessageType.Success;
                MessageBody = "Vessel has been deleted successfully";
            }

            else
            {
                MessageIcon = MessageType.Error;
                MessageBody = "Vessel could not be deleted!";
            }

            return RedirectToPage();
        }
    }
}