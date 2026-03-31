using Arena.Application.DTOs;
using Arena.Domain.Entities;
using Arena.Domain.Repositories;

namespace Arena.Application.Services;

public class BattleAppService
{
    private readonly IBattleRepository _battleRepository;
    private readonly ICreatureRepository _creatureRepository;

    public BattleAppService(IBattleRepository battleRepository, ICreatureRepository creatureRepository)
    {
        _battleRepository = battleRepository;
        _creatureRepository = creatureRepository;
    }

    public async Task<List<BattleResponse>> GetAllAsync()
    {
        var battles = await _battleRepository.GetAllAsync();
        var creatures = await _creatureRepository.GetAllAsync();
        var creaturesById = creatures.ToDictionary(c => c.Id);
        return battles.Select(b => ToResponse(b, creaturesById)).ToList();
    }

    public async Task<BattleResponse?> GetByIdAsync(Guid id)
    {
        var battle = await _battleRepository.GetByIdAsync(id);
        if (battle is null) return null;

        var creatures = await _creatureRepository.GetAllAsync();
        var creaturesById = creatures.ToDictionary(c => c.Id);
        return ToResponse(battle, creaturesById);
    }

    public async Task<BattleResponse> StartBattleAsync(Guid creature1Id, Guid creature2Id)
    {
        var creature1 = await _creatureRepository.GetByIdAsync(creature1Id)
            ?? throw new ArgumentException($"Créature 1 introuvable (ID: {creature1Id}).");
        var creature2 = await _creatureRepository.GetByIdAsync(creature2Id)
            ?? throw new ArgumentException($"Créature 2 introuvable (ID: {creature2Id}).");

        // Capturer les infos créatures avant que le combat ne mute leurs HP
        var c1Response = ToCreatureResponse(creature1);
        var c2Response = ToCreatureResponse(creature2);

        var battle = Battle.Run(creature1, creature2);
        await _battleRepository.AddAsync(battle);

        var winnerResponse = battle.WinnerId == creature1Id ? c1Response : c2Response;

        return new BattleResponse(
            battle.Id, battle.Creature1Id, battle.Creature2Id,
            battle.WinnerId, battle.PlayedAt,
            c1Response, c2Response, winnerResponse,
            battle.Logs.Select(l => new BattleLogResponse(l.Id, l.Turn, l.Description)).ToList());
    }

    private static BattleResponse ToResponse(Battle b, Dictionary<Guid, Creature> creatures)
    {
        creatures.TryGetValue(b.Creature1Id, out var c1);
        creatures.TryGetValue(b.Creature2Id, out var c2);
        Creature? winner = b.WinnerId.HasValue && creatures.TryGetValue(b.WinnerId.Value, out var w) ? w : null;

        return new BattleResponse(
            b.Id, b.Creature1Id, b.Creature2Id, b.WinnerId, b.PlayedAt,
            c1 is not null ? ToCreatureResponse(c1) : null!,
            c2 is not null ? ToCreatureResponse(c2) : null!,
            winner is not null ? ToCreatureResponse(winner) : null,
            b.Logs.OrderBy(l => l.Turn).Select(l => new BattleLogResponse(l.Id, l.Turn, l.Description)).ToList());
    }

    private static CreatureResponse ToCreatureResponse(Creature c) =>
        new(c.Id, c.Name, c.Type.ToString(), c.MaxHp, c.Attack, c.Defense, c.SpecialPower);
}
