using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Account
{
    [AllowAnonymous]
    public class LockedModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}

