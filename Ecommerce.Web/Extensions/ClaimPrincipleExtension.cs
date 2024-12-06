using System.Security.Claims;

namespace Ecommerce.Web.Extensions;

public static class ClaimPrincipleExtension
{
    //get user email
    public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email);
    }
}
