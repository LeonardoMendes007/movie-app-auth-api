
using MovieApp.AuthApi.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.AuthApi.Identity.Interfaces.Services;
public interface IRefreshTokenService
{
    Task RegisterRefreshTokenAsync(string refreshToken, DateTime refreshTokenExpiryTime, Credential credential);

    Task<AuthResult> ValidateRefreshTokenAsync(string userName, string refreshToken);
}
