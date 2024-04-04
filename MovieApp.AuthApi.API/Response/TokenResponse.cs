namespace MovieApp.AuthApi.API.Response;

public class TokenResponse
{
    public bool Authenticated { get; set; }
    public DateTime Expiration { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
