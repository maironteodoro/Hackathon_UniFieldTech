using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using UnifieldTech.Data;
using UnifieldTech.Models;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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
            // Validar o CPF antes de adicionar o cliente
            if (!ValidaCPF.validaCPF(cliente.CPF))
            {
                // Verificar se o CPF não é válido
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync("CPF inválido");
                return;
            }
            else if (db.Cliente.Any(c => c.CPF == cliente.CPF))
            {
                // Verificar se o CPF já está cadastrado no banco de dados
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsync("CPF já cadastrado");
                return;
            }


            cliente.Codigo = cliente.GerarStringAleatoria();
            db.Cliente.Add(cliente);
            await db.SaveChangesAsync();
            response.StatusCode = (int)HttpStatusCode.Created;
            await response.WriteAsync(JsonSerializer.Serialize(cliente));
           
            //return TypedResults.Created($"/api/Cliente/{cliente.ClienteID}", cliente);
            // Lógica para enviar o email
            //try
            //{
            //    // Configurações do servidor SMTP
            //    string smtpHost = "smtp.gmail.com";
            //    int smtpPort = 587;
            //    string smtpUsername = "email@gmail.com";
            //    string smtpPassword = "senha";

            //    // Configurações do email
            //    string fromEmail = "email@gmail.com";
            //    string toEmail = cliente.E_Mail;
            //    string subject = "Código de Verificação";
            //    string body = "Seu código de verificação é: " + cliente.Codigo;

            //    // Cria uma instância do cliente SMTP
            //    using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            //    {
            //        smtpClient.EnableSsl = true;
            //        smtpClient.UseDefaultCredentials = false;
            //        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

            //        // Cria a mensagem de email
            //        MailMessage emailMessage = new MailMessage(fromEmail, toEmail, subject, body);

            //        // Envia o email
            //        smtpClient.Send(emailMessage);

            //        Console.WriteLine("Email enviado com sucesso.");
            //    }

            //    response.StatusCode = (int)HttpStatusCode.Created;
            //    await response.WriteAsync(JsonSerializer.Serialize(cliente));
            //}
            //catch (Exception ex)
            //{
            //    response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    await response.WriteAsync("Ocorreu um erro ao enviar o email: " + ex.Message);
            //}
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
