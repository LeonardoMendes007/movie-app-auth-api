namespace MovieApp.AuthApi.API.Models;

public class TokenModel
{
    public DateTime Expiration { get; set; }
    public string Token { get; set; }
}
