using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Web.Pages.Accounting
{
    public class PayrollModel : BasePageModel
    {
        private readonly ILogger<PayrollModel> _logger;

        public PayrollModel(ILogger<PayrollModel> logger)
        {
            _logger = logger;
        }


        public void OnGet()
        {

        }
    }
}
