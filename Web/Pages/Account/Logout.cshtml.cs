using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Utility;
namespace Web.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IUserAccessor _userAccessor;
        private readonly IRepository _repository;
        public LogoutModel(SignInManager<AppUser> signInManager,
        ILogger<LogoutModel> logger,
        IUserAccessor userAccessor,
        IRepository repository)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userAccessor = userAccessor;
            _repository = repository;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("./Login");
            }
        }

    }
}
