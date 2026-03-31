using Arena.Application.DTOs;
using Arena.Application.UseCases.Creatures;
using Arena.Domain;

namespace Arena.API.Endpoints;

public static class CreatureEndpoints
{
    public static void MapCreatureEndpoints(this WebApplication app)
    {
        app.MapGet("/creatures", async (GetAllCreatures useCase) =>
        {
            var creatures = await useCase.ExecuteAsync();
            return Results.Ok(creatures);
        });

        app.MapGet("/creatures/{id:guid}", async (Guid id, GetCreatureById useCase) =>
        {
            var creature = await useCase.ExecuteAsync(id);
            return creature is not null ? Results.Ok(creature) : Results.NotFound();
        });

        app.MapPost("/creatures", async (CreateCreatureRequest request, CreateCreature useCase) =>
        {
            try
            {
                var created = await useCase.ExecuteAsync(request);
                return Results.Created($"/creatures/{created.Id}", created);
            }
            catch (Exception ex) when (ex is ArgumentException or DomainException)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapDelete("/creatures/{id:guid}", async (Guid id, DeleteCreature useCase) =>
        {
            var deleted = await useCase.ExecuteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
