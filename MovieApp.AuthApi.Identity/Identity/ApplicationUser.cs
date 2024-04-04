using Microsoft.AspNetCore.Identity;

namespace MovieApp.AuthApi.Identity.Identity;
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
