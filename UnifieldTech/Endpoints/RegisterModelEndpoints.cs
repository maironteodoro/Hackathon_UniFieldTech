using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UnifieldTech.ViewModels;
using System.Text.Json;
using System.Text;
using UnifieldTech.Services;

namespace UnifieldTech.Endpoints
{
    public static class RegisterModelEndpoints
    {
        private static IAuthenticate _authentication;

        public static void MapRegisterModelEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/RegisterModel");

            group.MapPost("/", async (HttpContext context, RegisterModel conte) =>
            {
                var model = await context.Request.ReadFromJsonAsync<RegisterModel>();

                if (model.Password != model.ConfirmPassword)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var errorResponse = new ModelStateDictionary();
                    errorResponse.AddModelError("ConfirmPassword", "As senhas não conferem");
                    var json = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(json, Encoding.UTF8);
                    return;
                }

                var result = await _authentication.RegisterUser(model.Email, model.Password);

                if (result)
                {
                    await context.Response.WriteAsync($"Usuário {model.Email} criado com sucesso");
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var errorResponse = new ModelStateDictionary();
                    errorResponse.AddModelError("CreateUser", "Registro inválido.");
                    var json = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            })
            .WithName("CreateRegisterModel")
            .WithOpenApi();
        }
    }
}