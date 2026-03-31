using Arena.Application.DTOs;
using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Battles;

public class GetAllBattles
{
    private readonly IBattleRepository _battleRepository;
    private readonly ICreatureRepository _creatureRepository;

    public GetAllBattles(IBattleRepository battleRepository, ICreatureRepository creatureRepository)
    {
        _battleRepository = battleRepository;
        _creatureRepository = creatureRepository;
    }

    public async Task<List<BattleResponse>> ExecuteAsync()
    {
        var battles = await _battleRepository.GetAllAsync();
        var creatures = await _creatureRepository.GetAllAsync();
        var creaturesById = creatures.ToDictionary(c => c.Id);
        return battles.Select(b => BattleResponse.FromDomain(b, creaturesById)).ToList();
    }
}
