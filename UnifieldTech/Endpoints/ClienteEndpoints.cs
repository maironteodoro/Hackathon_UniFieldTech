using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using UnifieldTech.Data;
using UnifieldTech.Models;

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
        .WithName("GetClienteById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int clienteid, Cliente cliente, UnifieldTechContext db) =>
        {
            var affected = await db.Cliente
                .Where(model => model.ClienteID == clienteid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ClienteID, cliente.ClienteID)
                  .SetProperty(m => m.NomeCliente, cliente.NomeCliente)
                  .SetProperty(m => m.CPF, cliente.CPF)
                  .SetProperty(m => m.E_Mail, cliente.E_Mail)
                  .SetProperty(m => m.DataNacs, cliente.DataNacs)
                  .SetProperty(m => m.Password, cliente.Password)
                  .SetProperty(m => m.Codigo, cliente.Codigo)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCliente")
        .WithOpenApi();

        group.MapPost("/", async (Cliente cliente, UnifieldTechContext db) =>
        {
            cliente.Codigo = cliente.GerarStringAleatoria();
            db.Cliente.Add(cliente);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/Cliente/{cliente.ClienteID}", cliente);
        })
        .WithName("CreateCliente")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int clienteid, UnifieldTechContext db) =>
        {
            var affected = await db.Cliente
                .Where(model => model.ClienteID == clienteid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCliente")
        .WithOpenApi();
    }
}
