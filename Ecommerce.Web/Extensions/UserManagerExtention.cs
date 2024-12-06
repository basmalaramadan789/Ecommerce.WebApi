using Ecommerce.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Web.Extensions;

public static class UserManagerExtention
{
    public static async Task<ApplicationUser> FindUserByClaimsPrincipleWithAddress(this UserManager<ApplicationUser> userManager,
            ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        return await userManager.Users.Include(x => x.Address)
            .SingleOrDefaultAsync(x => x.Email == email);
    }

    public static async Task<ApplicationUser> FindByEmailFromClaimsPrincipal(this UserManager<ApplicationUser> userManager,
       ClaimsPrincipal user)
    {
        return await userManager.Users.SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
    }
    // get user id
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier); // This will return the user ID from the token
    }
}
