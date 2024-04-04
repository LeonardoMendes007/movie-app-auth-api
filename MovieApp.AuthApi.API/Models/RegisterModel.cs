using System.ComponentModel.DataAnnotations;

namespace MovieApp.AuthApi.API.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
