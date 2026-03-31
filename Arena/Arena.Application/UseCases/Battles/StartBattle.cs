using Arena.Application.DTOs;
using Arena.Domain.Entities;
using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Battles;

public class StartBattle
{
    private readonly IBattleRepository _battleRepository;
    private readonly ICreatureRepository _creatureRepository;

    public StartBattle(IBattleRepository battleRepository, ICreatureRepository creatureRepository)
    {
        _battleRepository = battleRepository;
        _creatureRepository = creatureRepository;
    }

    public async Task<BattleResponse> ExecuteAsync(Guid creature1Id, Guid creature2Id)
    {
        var creature1 = await _creatureRepository.GetByIdAsync(creature1Id)
            ?? throw new ArgumentException($"Créature 1 introuvable (ID: {creature1Id}).");
        var creature2 = await _creatureRepository.GetByIdAsync(creature2Id)
            ?? throw new ArgumentException($"Créature 2 introuvable (ID: {creature2Id}).");

        // Capturer les infos créatures avant que le combat ne mute leurs HP
        var c1Response = CreatureResponse.FromDomain(creature1);
        var c2Response = CreatureResponse.FromDomain(creature2);

        var battle = Battle.Run(creature1, creature2);
        await _battleRepository.AddAsync(battle);

        var winnerResponse = battle.WinnerId == creature1Id ? c1Response : c2Response;

        return new BattleResponse(
            battle.Id, battle.Creature1Id, battle.Creature2Id,
            battle.WinnerId, battle.PlayedAt,
            c1Response, c2Response, winnerResponse,
            battle.Logs.Select(l => new BattleLogResponse(l.Id, l.Turn, l.Description)).ToList());
    }
}
