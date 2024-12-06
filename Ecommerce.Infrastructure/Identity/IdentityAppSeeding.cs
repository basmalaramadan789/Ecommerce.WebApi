using Ecommerce.Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Identity;
public class IdentityAppSeeding
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        var roles = new List<string> { "Admin", "User" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName,
                    IsDefault = roleName == "User"
                });
            }
        }
    }

    public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        if (!userManager.Users.Any()) // Only create if no users exist
        {
            var user = new ApplicationUser()
            {
                DisplayName = "Basmala",
                Email = "Basmala@gmail.com",
                UserName = "Basmala@gmail.com",
                Address = new Address
                {
                    FirstName = "Basmala",
                    LastName = "Ramadan",
                    Streat = "kkkk",
                    City = "Giza",
                    State = "EG",
                    ZipCode = "1155"
                }
            };

            var result = await userManager.CreateAsync(user, "P@ssw0rd");

            if (result.Succeeded)
            {
                // Assign the Admin role to the user
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}