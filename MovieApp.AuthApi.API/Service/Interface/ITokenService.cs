using MovieApp.AuthApi.API.Models;
using MovieApp.AuthApi.Identity.Model;
using System.Security.Claims;

namespace MovieApp.AuthApi.API.Service.Interface;

public interface ITokenService
{
    TokenModel GenerateAccessToken(Credential credential);
    RefreshTokenModel GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

}
