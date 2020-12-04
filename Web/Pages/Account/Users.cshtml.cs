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
using static Utility.UtilityFunctions;
using Microsoft.AspNetCore.Identity;

namespace Web.Pages.Account
{
    public class UsersModel : BasePageModel
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UsersModel(IRepository repository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<AppUserDto> AppUsers { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var appUsers = await _repository.GetAll<AppUser>();

            AppUsers = _mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserDto>>(appUsers);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAppUserAsync(AppUserInputModel appUserInput)
        {
            MessageTitle = "New AppUser Creation";

            var newAppUser = new AppUser
            {
                FirstName = appUserInput.FirstName.ToTitleCase(),
                LastName = appUserInput.LastName.ToTitleCase(),
                OtherName = appUserInput.OtherName.ToTitleCase(),
                UserName = appUserInput.Username
            };

            newAppUser.StaffId = await _repository.GenerateNewUserStaffId(newAppUser);

            var password = GenerateRandomPassword(7);

            var result = await _userManager.CreateAsync(newAppUser, password);

            if (result.Succeeded)
            {

                SetNotificationMessageAndIcon(string.Format("<p>AppUser, {0} has been successfully created. <br>New Password is <span class='text-primary password'> {1} </span>"
                    + "<b>NB: Kindly copy the password as it will disappear on clicking ok</b></p><br>",
                newAppUser.Name, password), MessageType.Success);

            }

            else
            {
                SetNotificationMessageAndIcon("AppUser could not be added!", MessageType.Error);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAppUserAsync(AppUserInputModel appUserInput)
        {
            MessageTitle = "AppUser Update";
            var appUser = await _repository.Get<AppUser>(appUserInput.Id);

            if (appUser == null)
            {
                SetNotificationMessageAndIcon("AppUser could not be updated!", MessageType.Error);

                return RedirectToPage();
            }

            appUser.FirstName = appUserInput.FirstName.ToTitleCase();
            appUser.LastName = appUserInput.LastName.ToTitleCase();
            appUser.OtherName = appUserInput.OtherName.ToTitleCase();


            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("AppUser has been updated successfully", MessageType.Success);
            }

            else
            {
                SetNotificationMessageAndIcon("AppUser could not be updated!", MessageType.Error);
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnGetEnableOrDisableAppUserAsync(string id)
        {
            MessageTitle = "AppUser Status Update";
            var appUser = await _repository.Get<AppUser>(id);

            appUser.LockoutEnd = (appUser.Locked) ? DateTime.Now : DateTime.Now.AddYears(1000);

            var clause = (!appUser.Locked) ? "active" : "inactive";

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon(string.Format("AppUser, {1} has been successfully set {0}",
                    clause, appUser.Name), MessageType.Success);
            }
            else
            {
                SetNotificationMessageAndIcon(string.Format("AppUser {1} could not be set {0}",
                    clause, appUser.Name), MessageType.Error);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetResetPasswordAsync(string id)
        {
            MessageTitle = "AppUser Password Reset";
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                SetNotificationMessageAndIcon("<p>Error Resetting AppUser Password.</p>",
                    MessageType.Error);

                return Page();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var password = GenerateRandomPassword(7);

            var result = await _userManager.ResetPasswordAsync(user, code, password);

            if (result.Succeeded)
            {
                SetNotificationMessageAndIcon(string.Format("<p>AppUser, {0} ({1})'s password has been successfully reset<br><br><b>New Password is:"
                 + " <span class='text-primary password'> {2} </span></b><br><br>" +
                 "<b>NB: Kindly copy the password as it will disappear on clicking ok</b></p>",
                 user.Name, user.UserName, password), MessageType.Success);

                user.RecommendedToChangePassword = true;

                await _repository.SaveAll();
            }

            else
            {
                SetNotificationMessageAndIcon("<p>Error Resetting AppUser's Password.</p>",
                   MessageType.Error);
            }

            return RedirectToPage();
        }

    }
}