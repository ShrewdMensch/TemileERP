using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var dateOfRegistration = DateTime.Now;

            if (!await roleManager.Roles.AnyAsync())
            {
                List<IdentityRole> roles = new List<IdentityRole>
                {
                    new IdentityRole(UserRoles.SystemAdministrator),
                    new IdentityRole(UserRoles.AccountOfficer)
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!await userManager.Users.AnyAsync())
            {
                List<AppUser> users = new List<AppUser>
                {
                    new AppUser
                    {
                        FirstName = "Test",
                        LastName = "Admin",
                        UserName = "admin",
                        Designation = UserRoles.SystemAdministrator,
                        StaffId = String.Format("{0}-{1}{2:D2}{3:D2}{4:D4}", "ADM",
                        dateOfRegistration.Year, dateOfRegistration.Month, dateOfRegistration.Day, 7)
                    },
                    new AppUser
                    {
                        FirstName = "Test",
                        LastName = "Account",
                        UserName = "account",
                        Designation = UserRoles.AccountOfficer,
                        StaffId = String.Format("{0}-{1}{2:D2}{3:D2}{4:D4}", "AO",
                        dateOfRegistration.Year, dateOfRegistration.Month, dateOfRegistration.Day, 7)
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, user.UserName);
                    await userManager.AddToRoleAsync(user, user.Designation);
                }
            }

        }
    }
}