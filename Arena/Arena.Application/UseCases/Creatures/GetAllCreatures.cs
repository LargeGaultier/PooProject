using Arena.Application.DTOs;
using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Creatures;

public class GetAllCreatures
{
    private readonly ICreatureRepository _creatureRepository;

    public GetAllCreatures(ICreatureRepository creatureRepository)
    {
        _creatureRepository = creatureRepository;
    }

    public async Task<List<CreatureResponse>> ExecuteAsync()
    {
        var creatures = await _creatureRepository.GetAllAsync();
        return creatures.Select(CreatureResponse.FromDomain).ToList();
    }
}
