using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Threading.Tasks;
using Utility;
using Utility.Extensions;
using Utility.InputModels;
using static Utility.UtilityClasses;

namespace Web.Pages.CompanyManagement
{
    [Authorize(Roles = UserRoles.SystemAdministrator)]
    public class CompanyInformationModel : BasePageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public CompanyInformation CompanyInformation { get; set; }

        public CompanyInformationModel(IRepository repository)
        {
            _repository = repository;
        }

        public async Task OnGet()
        {
            CompanyInformation = await _repository.GetCompanyInformation();

        }

        public async Task<IActionResult> OnPost()
        {
            MessageTitle = "Company Information Update";

            var companyInfoInDb = await _repository.Get<CompanyInformation>(CompanyInformation.Id);

            if (companyInfoInDb == null)
            {
                CompanyInformation.Country = CompanyInformation.Country.ToTitleCase() ?? "Nigeria";

                _repository.Add(CompanyInformation);
            }

            else
            {
                companyInfoInDb.Name = CompanyInformation.Name.ToTitleCase();
                companyInfoInDb.State = CompanyInformation.State.ToTitleCase();
                companyInfoInDb.Telephone = CompanyInformation.Telephone.ToTitleCase();
                companyInfoInDb.Address = CompanyInformation.Address.ToTitleCase();
                companyInfoInDb.City = CompanyInformation.City.ToTitleCase();
                companyInfoInDb.Email = CompanyInformation.Email;
                companyInfoInDb.Country = CompanyInformation.Country.ToTitleCase() ?? "Nigeria";
            }

            if (await _repository.SaveAll())
            {
                SetNotificationMessageAndIcon("Company Information updated succesfully", MessageType.Success);
            }
            else
            {
                SetNotificationMessageAndIcon("Company Information could not be updated", MessageType.Error);
            }

            return RedirectToPage();
        }
    }
}
