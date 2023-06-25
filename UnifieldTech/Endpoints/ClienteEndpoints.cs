using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using UnifieldTech.Data;
using UnifieldTech.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using UnifieldTech.ViewModels;
using System.Net;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.Mail;

namespace UnifieldTech.Endpoints
{
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
                      .SetProperty(m => m.CelularN, cliente.CelularN)
                      .SetProperty(m => m.E_Mail, cliente.E_Mail)
                      .SetProperty(m => m.DataNacs, cliente.DataNacs)
                      .SetProperty(m => m.Password, BCrypt.Net.BCrypt.HashPassword(cliente.Password))
                    );

                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
            .WithName("UpdateCliente")
            .WithOpenApi();

            group.MapPost("/", async (HttpResponse response, Cliente cliente, UnifieldTechContext db) =>
            {
                // Validar a idade mínima
                int idadeMinima = 18;
                DateTime dataAtual = DateTime.Now;
                int idade = dataAtual.Year - cliente.DataNacs.Year;
                if (cliente.DataNacs > dataAtual.AddYears(-idade))
                    idade--;

                if (idade < idadeMinima)
                {
                    // Retornar uma resposta de erro informando a idade mínima requerida
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await response.WriteAsJsonAsync($"Você deve ter no mínimo {idadeMinima} anos para realizar esta operação.");
                }

                MessageWhatsApp mw = new MessageWhatsApp();
                CpfConfig cp = new CpfConfig();

                HttpResponse cpfValidationResponse = await cp.ValidarCPFCliente(cliente.CPF, response, db);
                Object c = cpfValidationResponse.StatusCode;
                if (c.GetHashCode() != (int)HttpStatusCode.OK)
                {
                    c = response.WriteAsJsonAsync(cpfValidationResponse.ToString());
                    return c;
                }

                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "unifieldtechsolucoes@gmail.com";
                string smtpPassword = "mnvkbygahorwpdhw";

                // Criar cliente SMTP
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                // Habilitar conexão segura (SSL/TLS)
                smtpClient.EnableSsl = true;

                // Criar mensagem de e-mail
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername); // Use o mesmo e-mail configurado em smtpUsername
                mailMessage.To.Add(cliente.E_Mail);
                mailMessage.Subject = "Token de Verificação";

                cliente.Codigo = cliente.GerarStringAleatoria();
                mailMessage.Body = "Esse é seu código de validação: " + cliente.Codigo;
                cliente.Password = BCrypt.Net.BCrypt.HashPassword(cliente.Password);

                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();

                // Enviar e-mail
                smtpClient.Send(mailMessage);

                mw.EnviarCodigoValidacao(cliente.CelularN, cliente.Codigo);

                return TypedResults.Created($"/api/cliente/{cliente.ClienteID}", cliente);
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
}
