using System.Security.Claims;
using System.Collections.Generic;
using Domain;

namespace Utility
{
    public interface IUserAccessor
    {
        AppUser GetCurrentUser();
        AppUser GetUserFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
        string GetCurrentUsername();
        string GetCurrentUserId();
        IList<string> GetCurrentUserRoles();

        string GetCurrentUserFirstRole();
        string GetUserFirstRole(AppUser user);
    }
}