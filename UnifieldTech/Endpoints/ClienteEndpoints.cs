using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using UnifieldTech.Data;
using UnifieldTech.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using UnifieldTech.ViewModels;
using System.Net;
using Microsoft.Identity.Client;

namespace UnifieldTech.Endpoints
{
    public static class ClienteEndpoints
    {
        public static void MapClienteEndpoints(this IEndpointRouteBuilder routes)
        {
            MessageWhatsApp mw = new MessageWhatsApp();
            CpfConfig cp = new CpfConfig();
            Email em = new Email();
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

                HttpResponse idadeValidationResponse = await new Idade().MaiorIdade(cliente.DataNacs, response);
                if (idadeValidationResponse.StatusCode != (int)HttpStatusCode.OK)
                {
                    return idadeValidationResponse;
                }

                HttpResponse cpfValidationResponse = await cp.ValidarCPFCliente(cliente.CPF, response, db);
                Object c = cpfValidationResponse.StatusCode;
                if (c.GetHashCode() != (int)HttpStatusCode.OK)
                {
                    c = response.WriteAsJsonAsync(cpfValidationResponse.ToString());
                    return c;
                }

               
                cliente.Codigo = cliente.GerarStringAleatoria();
                cliente.Password = BCrypt.Net.BCrypt.HashPassword(cliente.Password);

                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();             

                mw.EnviarCodigoValidacao(cliente.CelularN, cliente.Codigo);
                em.EnviarEmail(cliente.E_Mail, cliente.Codigo);
                
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
