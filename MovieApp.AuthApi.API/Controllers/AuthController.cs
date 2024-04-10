using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.AuthApi.API.Request;
using MovieApp.AuthApi.API.Response;
using MovieApp.AuthApi.API.Service;
using MovieApp.AuthApi.API.Service.Interface;
using MovieApp.AuthApi.Identity.Interfaces.Services;
using MovieApp.AuthApi.Identity.Model;
using System.Net;

namespace MovieApp.API.Controllers;
[Route("api/auth")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService; 
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ITokenService tokenService, IRefreshTokenService refreshTokenService, ILogger<AuthController> logger)
    {
        _logger = logger;
        _authService = authService;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerModel)
    {
        var result = await _authService.RegisterAccount(registerModel.UserName, registerModel.Email, registerModel.Password);

        var tokenModel = _tokenService.GenerateAccessToken(result.Credential);

        var refreshTokenModel = _tokenService.GenerateRefreshToken();

        await _refreshTokenService.RegisterRefreshTokenAsync(refreshTokenModel.RefreshToken, refreshTokenModel.RefreshTokenValidityInHours, result.Credential);

        return Ok(ResponseBase<TokenResponse>.ResponseBaseFactory(new TokenResponse
        {
            Authenticated = true,
            Expiration = tokenModel.Expiration,
            AccessToken = tokenModel.Token,
            RefreshToken = refreshTokenModel.RefreshToken
        }, HttpStatusCode.OK));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInRequest signInModel)
    {
        var result = await _authService.SignInAsync(signInModel.Email, signInModel.Password);

        if (!result.IsAuthenticated || result.Credential is null)
            return Unauthorized(ResponseBase.ResponseBaseFactory(HttpStatusCode.Unauthorized, "Username or Password is invalid!"));

        var tokenModel = _tokenService.GenerateAccessToken(result.Credential);

        var refreshTokenModel = _tokenService.GenerateRefreshToken();

        await _refreshTokenService.RegisterRefreshTokenAsync(refreshTokenModel.RefreshToken, refreshTokenModel.RefreshTokenValidityInHours, result.Credential);

        return Ok(ResponseBase<TokenResponse>.ResponseBaseFactory(new TokenResponse
        {
            Authenticated = true,
            Expiration = tokenModel.Expiration,
            AccessToken = tokenModel.Token,
            RefreshToken = refreshTokenModel.RefreshToken
        }, HttpStatusCode.OK));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest tokenRequest)
    {
        if(tokenRequest == null)
            return BadRequest("Invalid tokens");

        string acessToken = tokenRequest.AccessToken;
        string refreshToken = tokenRequest.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken);

        if (principal == null)
            return BadRequest("Invalid access token/refresh token");

        var userName = principal.Identity.Name;

        var result = await _refreshTokenService.ValidateRefreshTokenAsync(userName,refreshToken);

        var tokenModel = _tokenService.GenerateAccessToken(result.Credential);

        var refreshTokenModel = _tokenService.GenerateRefreshToken();

        await _refreshTokenService.RegisterRefreshTokenAsync(refreshTokenModel.RefreshToken, refreshTokenModel.RefreshTokenValidityInHours, result.Credential);

        return Ok(ResponseBase<TokenResponse>.ResponseBaseFactory(new TokenResponse
        {
            Authenticated = true,
            Expiration = tokenModel.Expiration,
            AccessToken = tokenModel.Token,
            RefreshToken = refreshTokenModel.RefreshToken
        }, HttpStatusCode.OK));
    }

}
