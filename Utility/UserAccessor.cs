using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Utility
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public UserAccessor(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }
        public string GetCurrentUsername()
        {
            return GetCurrentUser().UserName;
        }
        public AppUser GetCurrentUser()
        {
            var user = _userManager.FindByIdAsync(GetCurrentUserId()).Result;

            return user;
        }
        public AppUser GetUserFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier).Value;

            return _userManager.FindByIdAsync(userId).Result;
        }

        public string GetUserFirstRole(AppUser user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;

            return userRoles.FirstOrDefault();
        }
        public string GetCurrentUserFirstRole()
        {
            return GetCurrentUserRoles().FirstOrDefault();
        }

        public IList<string> GetCurrentUserRoles()
        {
            var user = GetCurrentUser();
            var userRoles = _userManager.GetRolesAsync(user).Result;

            return userRoles;
        }

    }
}