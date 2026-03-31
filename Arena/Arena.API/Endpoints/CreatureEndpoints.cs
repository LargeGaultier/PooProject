using Arena.Business;
using Arena.Business.DTOs;

namespace Arena.API.Endpoints;

public static class CreatureEndpoints
{
    public static void MapCreatureEndpoints(this WebApplication app)
    {
        app.MapGet("/creatures", async (CreatureService service) =>
        {
            var creatures = await service.GetAllAsync();
            return Results.Ok(creatures);
        });

        app.MapGet("/creatures/{id:guid}", async (Guid id, CreatureService service) =>
        {
            var creature = await service.GetByIdAsync(id);
            return creature is not null ? Results.Ok(creature) : Results.NotFound();
        });

        app.MapPost("/creatures", async (CreateCreatureRequest request, CreatureService service) =>
        {
            try
            {
                var created = await service.CreateAsync(request);
                return Results.Created($"/creatures/{created.Id}", created);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapDelete("/creatures/{id:guid}", async (Guid id, CreatureService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
