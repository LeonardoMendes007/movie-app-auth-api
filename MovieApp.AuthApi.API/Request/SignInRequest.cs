using System.ComponentModel.DataAnnotations;

namespace MovieApp.AuthApi.API.Request;

public class SignInRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
