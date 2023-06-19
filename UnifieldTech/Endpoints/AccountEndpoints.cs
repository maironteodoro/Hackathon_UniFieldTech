using UnifieldTech.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnifieldTech.Services;

namespace UnifieldTech.Endpoints;

public static class AccountEndpoints
{
    private static IConfiguration _configuration;
    private static IAuthenticate _authentication;
    public static void MapAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/CreateUser").WithTags(nameof(Login));

        group.MapPost("/", async context =>
        {
            var model = await context.Request.ReadFromJsonAsync<RegisterModel>();

            if (model.Password != model.ConfirmPassword)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { ErrorMessage = "As senhas não conferem" });
                return;
            }

            var result = await _authentication.RegisterUser(model.Email, model.Password);

            if (result)
            {
                await context.Response.WriteAsJsonAsync($"Usuário {model.Email} criado com sucesso");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { ErrorMessage = "Registro inválido" });
            }
        })
        .WithName("GetAllLogin")
        .WithOpenApi();

        routes.MapPost("/api/LoginUser", async context =>
        {
            var userInfo = await context.Request.ReadFromJsonAsync<Login>();

            var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                var token = GenerateToken(_configuration, userInfo);
                await context.Response.WriteAsJsonAsync(token);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { ErrorMessage = "Login inválido" });
            }
        });
    }

    private static UserToken GenerateToken(IConfiguration configuration, Login userInfo)
    {
        var claims = new[]
        {
            new Claim("email", userInfo.Email),
            new Claim("meuToken", "token do macoratti"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(20);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
        };
    }
}
