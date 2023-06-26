using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using UnifieldTech.Data;
using UnifieldTech.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace UnifieldTech.Endpoints;

public static class FazendaEndpoints
{
    public static void MapFazendaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Fazenda").WithTags(nameof(Fazenda));

        group.MapGet("/", async (UnifieldTechContext db) =>
        {
            return await db.Fazenda.ToListAsync();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("GetAllFazendas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Fazenda>, NotFound>> (int fazendaid, UnifieldTechContext db) =>
        {
            return await db.Fazenda.AsNoTracking()
                .FirstOrDefaultAsync(model => model.FazendaID == fazendaid)
                is Fazenda model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("GetFazendaById")
        .WithOpenApi();

        group.MapGet("/buscar/cliente/cpf/{cpf}", async (string cpf, UnifieldTechContext db) =>
        {
            var cliente = await db.Cliente.AsNoTracking()
                .FirstOrDefaultAsync(model => model.CPF == cpf);

            if (cliente != null)
            {
                return cliente.ClienteID; // Retorna somente o ID do cliente
            }
            else
            {
                return 0;
            }
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("BuscarClienteIdPorCpf")
        .WithOpenApi();

        group.MapGet("/buscar/cliente/cpf/{cpf}/fazenda", async (HttpContext context, string cpf, UnifieldTechContext db) =>
        {
            var cliente = await db.Cliente.AsNoTracking()
                .FirstOrDefaultAsync(model => model.CPF == cpf);

            if (cliente != null)
            {
                var fazenda = await db.Fazenda.AsNoTracking()
                    .FirstOrDefaultAsync(f => f.ClienteID == cliente.ClienteID);

                if (fazenda != null)
                {
                    return Results.Ok(fazenda); // Retorna a fazenda encontrada
                }
                else
                {
                    return Results.NotFound(); // Retorna uma resposta indicando que a fazenda não foi encontrada
                }
            }
            else
            {
                return Results.NotFound(); // Retorna uma resposta indicando que o cliente não foi encontrado
            }
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("BuscarFazendaPorCpf")
        .WithOpenApi();



        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int fazendaid, Fazenda fazenda, UnifieldTechContext db) =>
        {
            Cliente c = new Cliente();
            var affected = await db.Fazenda
                .Where(model => model.FazendaID == fazendaid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.NomeFazenda, fazenda.NomeFazenda)
                  .SetProperty(m => m.Hectar, fazenda.Hectar)
                  .SetProperty(m => m.Cultivar, fazenda.Cultivar)
                  .SetProperty(m => m.Rua, fazenda.Rua)
                  .SetProperty(m => m.Num, fazenda.Num)
                  .SetProperty(m => m.Cidade, fazenda.Cidade)
                  .SetProperty(m => m.Estado, fazenda.Estado)
                  .SetProperty(m => m.Latitude, fazenda.Latitude)
                  .SetProperty(m => m.Longitude, fazenda.Longitude)
                  .SetProperty(m => m.TipoPlantio, fazenda.TipoPlantio)
                  .SetProperty(m => m.AreaMecanizada, fazenda.AreaMecanizada)
                  .SetProperty(m => m.ClienteID, fazenda.ClienteID)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("UpdateFazenda")
        .WithOpenApi();

        group.MapPost("/", async (Fazenda fazenda, UnifieldTechContext db) =>
        {
            db.Fazenda.Add(fazenda);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Fazenda/{fazenda.FazendaID}", fazenda);
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("CreateFazenda")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int fazendaid, UnifieldTechContext db) =>
        {
            var affected = await db.Fazenda
                .Where(model => model.FazendaID == fazendaid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme })
        .WithName("DeleteFazenda")
        .WithOpenApi();
    }
}
