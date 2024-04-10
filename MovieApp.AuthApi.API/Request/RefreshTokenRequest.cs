using System.ComponentModel.DataAnnotations;

namespace MovieApp.AuthApi.API.Request;

public class RefreshTokenRequest
{
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}
