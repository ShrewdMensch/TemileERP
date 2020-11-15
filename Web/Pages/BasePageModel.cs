using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class BasePageModel : PageModel
    {
        [TempData]
        public string MessageBody { get; set; }
        [TempData]
        public string MessageTitle { get; set; }
        [TempData]
        public string MessageIcon { get; set; }
    }
}