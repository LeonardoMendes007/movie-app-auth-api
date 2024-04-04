using MovieApp.AuthApi.Identity.IndentityConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MovieApp.AuthApi.API.Config;
using MovieApp.AuthApi.API.Service;
using System.Text;
using MovieApp.AuthApi.API.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using MovieApp.API.Middleware;
using MovieApp.AuthApi.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var tokenConfiguration = builder.Configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();
var secretKey = builder.Configuration["JWT:Key"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = tokenConfiguration.Audience,
            ValidIssuer = tokenConfiguration.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            )
        }
);

builder.Services.AddScoped<ITokenService, TokenService>(x => new TokenService(secretKey, tokenConfiguration));
builder.Services.AddApiIdentityConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
