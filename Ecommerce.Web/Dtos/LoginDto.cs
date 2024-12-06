using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Web.Dtos;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public string Password { get; set; }
}
