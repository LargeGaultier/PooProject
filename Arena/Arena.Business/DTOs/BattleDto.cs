using System.Linq.Expressions;
using Arena.DataAccess.Entities;

namespace Arena.Business.DTOs;

public record StartBattleRequest(Guid Creature1Id, Guid Creature2Id);

public record BattleResponse(
    Guid Id,
    Guid Creature1Id,
    Guid Creature2Id,
    Guid? WinnerId,
    DateTime PlayedAt,
    CreatureResponse Creature1,
    CreatureResponse Creature2,
    CreatureResponse? Winner,
    List<BattleLogResponse> Logs
)
{
    public static Expression<Func<BattleEntity, BattleResponse>> Projection =>
        b => new BattleResponse(
            b.Id,
            b.Creature1Id,
            b.Creature2Id,
            b.WinnerId,
            b.PlayedAt,
            new CreatureResponse(b.Creature1.Id, b.Creature1.Name, b.Creature1.Type,
                b.Creature1.MaxHp, b.Creature1.Attack, b.Creature1.Defense, b.Creature1.SpecialPower),
            new CreatureResponse(b.Creature2.Id, b.Creature2.Name, b.Creature2.Type,
                b.Creature2.MaxHp, b.Creature2.Attack, b.Creature2.Defense, b.Creature2.SpecialPower),
            b.Winner == null ? null : new CreatureResponse(b.Winner.Id, b.Winner.Name, b.Winner.Type,
                b.Winner.MaxHp, b.Winner.Attack, b.Winner.Defense, b.Winner.SpecialPower),
            b.Logs.OrderBy(l => l.Turn).Select(l => new BattleLogResponse(l.Id, l.Turn, l.Description)).ToList()
        );
}

public record BattleLogResponse(
    Guid Id,
    int Turn,
    string Description
);
