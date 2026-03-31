using Arena.Application.DTOs;
using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Creatures;

public class GetCreatureById
{
    private readonly ICreatureRepository _creatureRepository;

    public GetCreatureById(ICreatureRepository creatureRepository)
    {
        _creatureRepository = creatureRepository;
    }

    public async Task<CreatureResponse?> ExecuteAsync(Guid id)
    {
        var creature = await _creatureRepository.GetByIdAsync(id);
        return creature is null ? null : CreatureResponse.FromDomain(creature);
    }
}
