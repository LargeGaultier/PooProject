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
);

public record BattleLogResponse(
    Guid Id,
    int Turn,
    string Description
);
