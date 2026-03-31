using Arena.Application.DTOs;
using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Battles;

public class GetBattleById
{
    private readonly IBattleRepository _battleRepository;
    private readonly ICreatureRepository _creatureRepository;

    public GetBattleById(IBattleRepository battleRepository, ICreatureRepository creatureRepository)
    {
        _battleRepository = battleRepository;
        _creatureRepository = creatureRepository;
    }

    public async Task<BattleResponse?> ExecuteAsync(Guid id)
    {
        var battle = await _battleRepository.GetByIdAsync(id);
        if (battle is null) return null;

        var creatures = await _creatureRepository.GetAllAsync();
        var creaturesById = creatures.ToDictionary(c => c.Id);
        return BattleResponse.FromDomain(battle, creaturesById);
    }
}
