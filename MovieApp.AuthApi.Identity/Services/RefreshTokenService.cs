using Microsoft.AspNetCore.Identity;
using MovieApp.AuthApi.Identity.Exceptions;
using MovieApp.AuthApi.Identity.Identity;
using MovieApp.AuthApi.Identity.Interfaces.Services;
using MovieApp.AuthApi.Identity.Model;
using System.Security.Claims;

namespace MovieApp.AuthApi.Identity.Services;
public class RefreshTokenService : IRefreshTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RefreshTokenService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterRefreshTokenAsync(string refreshToken, DateTime refreshTokenExpiryTime, Credential credential)
    {
        var appUser = await _userManager.FindByEmailAsync(credential.Email);
        if (appUser != null)
        { 
            appUser.RefreshToken = refreshToken;
            appUser.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _userManager.UpdateAsync(appUser);
        }
    }

    public async Task<AuthResult> ValidateRefreshTokenAsync(string userName, string refreshToken)
    {
        var appUser = await _userManager.FindByNameAsync(userName);

        if(appUser == null || appUser.RefreshToken != refreshToken
                           || appUser.RefreshTokenExpiryTime <= DateTime.Now) 
        {
            throw new InvalidRefreshTokenException();
        }

        var credential = new Credential { id = appUser.Id, Email = appUser.Email };

        var userRoles = await _userManager.GetRolesAsync(appUser);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim("AccountId", appUser.Id)
            };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        credential.Claims = claims;

        return new AuthResult(credential, true);
    }
}
