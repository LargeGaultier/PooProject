using Arena.Application.DTOs;
using Arena.Application.UseCases.Battles;

namespace Arena.API.Endpoints;

public static class BattleEndpoints
{
    public static void MapBattleEndpoints(this WebApplication app)
    {
        app.MapGet("/battles", async (GetAllBattles useCase) =>
        {
            var battles = await useCase.ExecuteAsync();
            return Results.Ok(battles);
        });

        app.MapGet("/battles/{id:guid}", async (Guid id, GetBattleById useCase) =>
        {
            var battle = await useCase.ExecuteAsync(id);
            return battle is not null ? Results.Ok(battle) : Results.NotFound();
        });

        app.MapPost("/battles", async (StartBattleRequest request, StartBattle useCase) =>
        {
            try
            {
                var battle = await useCase.ExecuteAsync(request.Creature1Id, request.Creature2Id);
                return Results.Created($"/battles/{battle.Id}", battle);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}
