using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using UnifieldTech.Data;
using UnifieldTech.Models;

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
        .WithName("UpdateCelular")
        .WithOpenApi();

        group.MapPost("/", async (Celular celular, UnifieldTechContext db) =>
        {
            db.Celular.Add(celular);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Celular/{celular.CelularID}", celular);
        })
        .WithName("CreateCelular")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int celularid, UnifieldTechContext db) =>
        {
            var affected = await db.Celular
                .Where(model => model.CelularID == celularid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCelular")
        .WithOpenApi();
    }
}
