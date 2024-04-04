
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MovieApp.AuthApi.Identity.Exceptions;
using MovieApp.AuthApi.Identity.Identity;
using MovieApp.AuthApi.Identity.Interfaces.Services;
using MovieApp.AuthApi.Identity.Model;
using System.Security.Claims;

namespace MovieApp.AuthApi.Identity.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleStore;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleStore)
    {
        _userManager = userManager;
        _roleStore = roleStore;
    }

    public async Task<AuthResult> RegisterAccount(string userName, string email, string password)
    {
        if ((await _userManager.FindByNameAsync(userName)) is not null)
            throw new UserNameAlreadyExistsException();

        if ((await _userManager.FindByEmailAsync(email)) is not null)
            throw new EmailAlreadyExistsException();

        var appUser = new ApplicationUser()
        {
            UserName = userName,
            Email = email
        };

        var identityResult = await _userManager.CreateAsync(appUser, password);

        if (!identityResult.Succeeded)
            return new AuthResult();

        return await SignInAsync(email, password);
    }

    public async Task<AuthResult> SignInAsync(string email, string password)
    {
        var appUser = await _userManager.FindByEmailAsync(email);

        if (appUser != null && await _userManager.CheckPasswordAsync(appUser, password))
        {
            var credential = new Credential { id = appUser.Id, UserName = appUser.UserName, Email = appUser.Email };

            var userRoles = await _userManager.GetRolesAsync(appUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim("Id", appUser.Id)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            credential.Claims = claims;

            return new AuthResult(credential, true);
        }

        return new AuthResult();
    }

}
