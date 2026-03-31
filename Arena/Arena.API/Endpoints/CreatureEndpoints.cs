using Arena.Application.DTOs;
using Arena.Application.Services;
using Arena.Domain;

namespace Arena.API.Endpoints;

public static class CreatureEndpoints
{
    public static void MapCreatureEndpoints(this WebApplication app)
    {
        app.MapGet("/creatures", async (CreatureAppService service) =>
        {
            var creatures = await service.GetAllAsync();
            return Results.Ok(creatures);
        });

        app.MapGet("/creatures/{id:guid}", async (Guid id, CreatureAppService service) =>
        {
            var creature = await service.GetByIdAsync(id);
            return creature is not null ? Results.Ok(creature) : Results.NotFound();
        });

        app.MapPost("/creatures", async (CreateCreatureRequest request, CreatureAppService service) =>
        {
            try
            {
                var created = await service.CreateAsync(request);
                return Results.Created($"/creatures/{created.Id}", created);
            }
            catch (Exception ex) when (ex is ArgumentException or DomainException)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapDelete("/creatures/{id:guid}", async (Guid id, CreatureAppService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
