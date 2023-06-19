using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using UnifieldTech.Data;
using UnifieldTech.Models;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace UnifieldTech.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cliente").WithTags(nameof(Cliente));

        group.MapGet("/", async (UnifieldTechContext db) =>
        {
            return await db.Cliente.ToListAsync();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("GetAllClientes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cliente>, NotFound>> (int clienteid, UnifieldTechContext db) =>
        {
            return await db.Cliente.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ClienteID == clienteid)
                is Cliente model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("GetClienteById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int clienteid, Cliente cliente, UnifieldTechContext db) =>
        {
            var affected = await db.Cliente
                .Where(model => model.ClienteID == clienteid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.NomeCliente, cliente.NomeCliente)
                  //.SetProperty(m => m.CPF, cliente.CPF)
                  .SetProperty(m => m.CelularN, cliente.CelularN)
                  .SetProperty(m => m.E_Mail, cliente.E_Mail)
                  .SetProperty(m => m.DataNacs, cliente.DataNacs)
                  .SetProperty(m => m.Password, cliente.Password)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("UpdateCliente")
        .WithOpenApi();


        group.MapPost("/", async (HttpRequest request, HttpResponse response, Cliente cliente, UnifieldTechContext db) =>
        {
            TwilioClient.Init("AC2c556c8c4bbc592728c527807458b033", "a523aa1d55bbd1c16a6b70d2c8b07f57");

            // Validar o CPF antes de adicionar o cliente
            //if (!ValidaCPF.validaCPF(cliente.CPF))
            //{
            //    // Verificar se o CPF não é válido
            //    response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    await response.WriteAsync("CPF inválido");
            //    return;
            //}
            //else if (db.Cliente.Any(c => c.CPF == cliente.CPF))
            //{
            //    // Verificar se o CPF já está cadastrado no banco de dados
            //    response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    await response.WriteAsync("CPF já cadastrado");
            //    return;
            //}

            cliente.Codigo = cliente.GerarStringAleatoria();

            db.Cliente.Add(cliente);
            await db.SaveChangesAsync();

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:+55{cliente.CelularN}"))
            {
                From = new PhoneNumber("whatsapp:+14155238886"),
                Body = "Esse é seu código de validação: " + cliente.Codigo
            };

            var message = MessageResource.Create(messageOptions);
            return TypedResults.Created($"/api/cliente/{cliente.ClienteID}", cliente + message.Sid);
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("CreateCliente")
        .WithOpenApi();



        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int clienteid, UnifieldTechContext db) =>
        {
            var affected = await db.Cliente
                .Where(model => model.ClienteID == clienteid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("DeleteCliente")
        .WithOpenApi();
    }
}
