namespace MovieApp.AuthApi.API.Models;

public class RefreshTokenModel
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenValidityInHours { get; set; }
}
