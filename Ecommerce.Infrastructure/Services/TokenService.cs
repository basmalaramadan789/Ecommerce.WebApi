using Ecommerce.Core.Identity;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
    {
        _config = config;

        // Fetch the signing key from configuration
        var key = _config["Token:key"];
        if (string.IsNullOrEmpty(key) || Encoding.UTF8.GetBytes(key).Length < 64)
        {
            throw new ArgumentException("The signing key must be at least 64 characters long for HMAC-SHA512.");
        }

        // Create the SymmetricSecurityKey
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        _userManager = userManager;

    }

    public string CreateToken(ApplicationUser user)
    {
        var roles = _userManager.GetRolesAsync(user).Result;

        // Get the user's roles
        // Define claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.DisplayName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        // Add roles to the claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); // Adding the role claim
        }


        // Create signing credentials
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        // Define the token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7), // Token expires in 7 days
            SigningCredentials = creds,
            Issuer = _config["Token:Issuer"] // Optional issuer claim
        };

        // Create and return the token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
