using Arena.Domain.Entities;

namespace Arena.Application.DTOs;

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
    public static BattleResponse FromDomain(Battle b, Dictionary<Guid, Creature> creatures)
    {
        creatures.TryGetValue(b.Creature1Id, out var c1);
        creatures.TryGetValue(b.Creature2Id, out var c2);
        Creature? winner = b.WinnerId.HasValue && creatures.TryGetValue(b.WinnerId.Value, out var w) ? w : null;

        return new BattleResponse(
            b.Id, b.Creature1Id, b.Creature2Id, b.WinnerId, b.PlayedAt,
            c1 is not null ? CreatureResponse.FromDomain(c1) : null!,
            c2 is not null ? CreatureResponse.FromDomain(c2) : null!,
            winner is not null ? CreatureResponse.FromDomain(winner) : null,
            b.Logs.OrderBy(l => l.Turn).Select(l => new BattleLogResponse(l.Id, l.Turn, l.Description)).ToList());
    }
};

public record BattleLogResponse(Guid Id, int Turn, string Description);
