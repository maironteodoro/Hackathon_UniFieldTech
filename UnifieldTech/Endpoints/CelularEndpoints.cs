using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using UnifieldTech.Data;
using UnifieldTech.Models;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UnifieldTech.Endpoints;

public static class CelularEndpoints
{
    public static void MapCelularEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Celular").WithTags(nameof(Celular));

        group.MapGet("/", async (UnifieldTechContext db) =>
        {
            return await db.Celular.ToListAsync();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("GetAllCelulars")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Celular>, NotFound>> (int celularid, UnifieldTechContext db) =>
        {
            return await db.Celular.AsNoTracking()
                .FirstOrDefaultAsync(model => model.CelularID == celularid)
                is Celular model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("GetCelularById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int celularid, Celular celular, UnifieldTechContext db) =>
        {
            var affected = await db.Celular
                .Where(model => model.CelularID == celularid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.CelularID, celular.CelularID)
                  .SetProperty(m => m.CelularN, celular.CelularN)
                  .SetProperty(m => m.ClienteID, celular.ClienteID)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("UpdateCelular")
        .WithOpenApi();

        group.MapPost("/", async (Celular celular, UnifieldTechContext db) =>
        {
            // Lógica para enviar SMS utilizando a biblioteca Twilio
            TwilioClient.Init("AC2c556c8c4bbc592728c527807458b033", "6788b7c2d6bdd9b8c4c77fe34f48df9f");

            db.Celular.Add(celular);
            await db.SaveChangesAsync();

            var cliente = await db.Cliente.FirstOrDefaultAsync(t => t.ClienteID == celular.ClienteID);
            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:+55{celular.CelularN}"))
            {
                From = new PhoneNumber("whatsapp:+14155238886"),
                Body = "Esse é seu código de validação: " + cliente.Codigo
            };
            var message = MessageResource.Create(messageOptions);
            return TypedResults.Created($"/api/Celular/{celular.CelularID}", celular + message.Sid);
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("CreateCelular")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int celularid, UnifieldTechContext db) =>
        {
            var affected = await db.Celular
                .Where(model => model.CelularID == celularid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("DeleteCelular")
        .WithOpenApi();
    }
}
