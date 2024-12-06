using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Web.Dtos;

public class RegisterDto
{
    [Required]
    public string DisplayName {  get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }

}
