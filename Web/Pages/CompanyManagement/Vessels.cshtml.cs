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

namespace Web.Pages.CompanyManagement
{
    public class VesselsModel : BasePageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public VesselsModel(IRepository repository, IMapper mapper, IUserAccessor userAccessor)
        {
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
                SetNotificationMessageAndIcon("Vessel has been added successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Vessel could not be added!", MessageType.Error);
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
                SetNotificationMessageAndIcon("Vessel could not be updated!", MessageType.Error);

                return RedirectToPage();
            }

            vessel.LastModified = DateTime.Now;
            vessel.ModifiedBy = currentUser;
            vessel.Name = vesselInput.Name.ToTitleCase();


            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Vessel has been updated successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Vessel could not be updated!", MessageType.Error);
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteVesselAsync(VesselInputModel vesselInput)
        {
            MessageTitle = "Vessel Deletion";
            var vessel = await _repository.Get<Vessel>(vesselInput.Id);

            if (vessel == null)
            {
                SetNotificationMessageAndIcon("Vessel could not be deleted!", MessageType.Error);

                return RedirectToPage();
            }

            _repository.Remove(vessel);

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Vessel has been deleted successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("Vessel could not be deleted!", MessageType.Error);
            }

            return RedirectToPage();
        }
    }
}