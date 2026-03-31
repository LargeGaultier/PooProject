using Arena.Business;
using Arena.Business.DTOs;

namespace Arena.API.Endpoints;

public static class BattleEndpoints
{
    public static void MapBattleEndpoints(this WebApplication app)
    {
        app.MapGet("/battles", async (BattleService service) =>
        {
            var battles = await service.GetAllAsync();
            return Results.Ok(battles);
        });

        app.MapGet("/battles/{id:guid}", async (Guid id, BattleService service) =>
        {
            var battle = await service.GetByIdAsync(id);
            return battle is not null ? Results.Ok(battle) : Results.NotFound();
        });

        app.MapPost("/battles", async (StartBattleRequest request, BattleService service) =>
        {
            try
            {
                var battle = await service.StartBattleAsync(request.Creature1Id, request.Creature2Id);
                return Results.Created($"/battles/{battle.Id}", battle);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}
