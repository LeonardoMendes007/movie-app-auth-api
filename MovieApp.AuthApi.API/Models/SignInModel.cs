﻿using System.ComponentModel.DataAnnotations;

namespace MovieApp.AuthApi.API.Models;

public class SignInModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
