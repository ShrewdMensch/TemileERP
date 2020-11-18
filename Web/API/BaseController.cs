using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class BaseController : ControllerBase
    {
        private IRepository _repository;
        private IMapper _mapper;
        private IUserAccessor _userAccessor;
        private UserManager<AppUser> _userManager;

        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
        protected IUserAccessor UserAccessor => _userAccessor ?? (_userAccessor = HttpContext.RequestServices.GetService<IUserAccessor>());
        protected IRepository Repository => _repository ?? (_repository = HttpContext.RequestServices.GetService<IRepository>());
        protected UserManager<AppUser> UserManager => _userManager ?? (_userManager = HttpContext.RequestServices.GetService<UserManager<AppUser>>());

    }
}