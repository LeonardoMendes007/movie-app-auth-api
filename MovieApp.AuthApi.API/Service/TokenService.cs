using Microsoft.IdentityModel.Tokens;
using MovieApp.AuthApi.API.Config;
using MovieApp.AuthApi.API.Models;
using MovieApp.AuthApi.API.Service.Interface;
using MovieApp.AuthApi.Identity.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieApp.AuthApi.API.Service;

public class TokenService : ITokenService
{
    private readonly string _secret;
    private readonly TokenConfiguration _tokenConfiguration;
    public TokenService(string secret, TokenConfiguration tokenConfiguration)
    {
        _secret = secret;
        _tokenConfiguration = tokenConfiguration;
    }
    public TokenModel GenerateAccessToken(Credential credential)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secret));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(_tokenConfiguration.TokenValidityInHours);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _tokenConfiguration.Issuer,
            audience: _tokenConfiguration.Audience,
            claims: credential.Claims,
            expires: expiration,
            signingCredentials: credentials
            );


        return new TokenModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }

    public RefreshTokenModel GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return new RefreshTokenModel()
        {
            RefreshToken = refreshToken,
            RefreshTokenValidityInHours = DateTime.Now.AddHours(_tokenConfiguration.RefreshTokenValidityInHours)
        };

    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _tokenConfiguration.Audience,
            ValidIssuer = _tokenConfiguration.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secret)
            ),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                    out SecurityToken securityToken);

        if(securityToken is not JwtSecurityToken jwtSecurityToken ||
                         !jwtSecurityToken.Header.Alg.Equals(
                             SecurityAlgorithms.HmacSha256,
                             StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
