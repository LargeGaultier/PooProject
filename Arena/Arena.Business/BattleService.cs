using Arena.Business.DTOs;
using Arena.DataAccess.Entities;
using Arena.DataAccess.Repositories;

namespace Arena.Business;

public class BattleService
{
    private readonly BattleRepository _battleRepository;
    private readonly CreatureRepository _creatureRepository;

    public BattleService(BattleRepository battleRepository, CreatureRepository creatureRepository)
    {
        _battleRepository = battleRepository;
        _creatureRepository = creatureRepository;
    }

    public async Task<List<BattleResponse>> GetAllAsync()
    {
        var entities = await _battleRepository.GetAllAsync();
        return entities.Select(ToDto).ToList();
    }

    public async Task<BattleResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _battleRepository.GetByIdAsync(id);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<BattleResponse> StartBattleAsync(Guid creature1Id, Guid creature2Id)
    {
        var creature1 = await _creatureRepository.GetByIdAsync(creature1Id)
            ?? throw new ArgumentException($"Créature 1 introuvable (ID: {creature1Id}).");

        var creature2 = await _creatureRepository.GetByIdAsync(creature2Id)
            ?? throw new ArgumentException($"Créature 2 introuvable (ID: {creature2Id}).");

        // Simulation du combat tour par tour
        int hp1 = creature1.MaxHp;
        int hp2 = creature2.MaxHp;
        int turn = 1;
        var logs = new List<BattleLogEntity>();

        logs.Add(new BattleLogEntity
        {
            Id = Guid.NewGuid(),
            Turn = 0,
            Description = $"Combat entre {creature1.Name} et {creature2.Name} !"
        });

        while (hp1 > 0 && hp2 > 0)
        {
            // Créature 1 attaque Créature 2
            int damage1 = Math.Max(1, creature1.Attack - creature2.Defense);
            hp2 -= damage1;
            logs.Add(new BattleLogEntity
            {
                Id = Guid.NewGuid(),
                Turn = turn,
                Description = $"Tour {turn} : {creature1.Name} attaque {creature2.Name} pour {damage1} dégâts. (HP {creature2.Name}: {Math.Max(0, hp2)})"
            });

            if (hp2 <= 0) break;

            // Créature 2 attaque Créature 1
            int damage2 = Math.Max(1, creature2.Attack - creature1.Defense);
            hp1 -= damage2;
            logs.Add(new BattleLogEntity
            {
                Id = Guid.NewGuid(),
                Turn = turn,
                Description = $"Tour {turn} : {creature2.Name} attaque {creature1.Name} pour {damage2} dégâts. (HP {creature1.Name}: {Math.Max(0, hp1)})"
            });

            turn++;
        }

        var winnerId = hp1 > 0 ? creature1.Id : creature2.Id;
        var winnerName = hp1 > 0 ? creature1.Name : creature2.Name;

        logs.Add(new BattleLogEntity
        {
            Id = Guid.NewGuid(),
            Turn = turn,
            Description = $"{winnerName} remporte le combat !"
        });

        var battle = new BattleEntity
        {
            Creature1Id = creature1Id,
            Creature2Id = creature2Id,
            WinnerId = winnerId,
            PlayedAt = DateTime.UtcNow,
            Logs = logs
        };

        var saved = await _battleRepository.AddAsync(battle);

        // Recharger avec les navigation properties
        var full = await _battleRepository.GetByIdAsync(saved.Id);
        return ToDto(full!);
    }

    private static BattleResponse ToDto(BattleEntity entity)
    {
        return new BattleResponse(
            entity.Id,
            entity.Creature1Id,
            entity.Creature2Id,
            entity.WinnerId,
            entity.PlayedAt,
            CreatureService.ToDto(entity.Creature1),
            CreatureService.ToDto(entity.Creature2),
            entity.Winner is null ? null : CreatureService.ToDto(entity.Winner),
            entity.Logs.Select(l => new BattleLogResponse(l.Id, l.Turn, l.Description)).ToList()
        );
    }
}
