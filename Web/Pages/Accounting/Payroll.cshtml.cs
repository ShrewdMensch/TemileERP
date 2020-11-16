using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Domain;
using Utility;
using Utility.DTOs;

namespace Web.Pages.Accounting
{
    public class PayrollModel : BasePageModel
    {
        private readonly ILogger<PayrollModel> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public PayrollModel(ILogger<PayrollModel> logger, IRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [BindProperty]
        public IEnumerable<PersonnelPayrollDto> Payrolls { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var payrolls = await _repository.GetAll<Payroll>();

            Payrolls = _mapper.Map<IEnumerable<Payroll>, IEnumerable<PersonnelPayrollDto>>(payrolls);

            return Page();
        }
    }
}
