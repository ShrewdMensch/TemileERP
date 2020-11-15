using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Web.Pages.Accounting
{
    public class CurrentParemetersModel : PageModel
    {
        private readonly ILogger<CurrentParemetersModel> _logger;

        public CurrentParemetersModel(ILogger<CurrentParemetersModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
