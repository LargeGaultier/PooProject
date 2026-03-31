using Arena.Application.DTOs;
using Arena.Domain;
using Arena.Domain.Entities;
using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Creatures;

public class CreateCreature
{
    private readonly ICreatureRepository _creatureRepository;

    public CreateCreature(ICreatureRepository creatureRepository)
    {
        _creatureRepository = creatureRepository;
    }

    public async Task<CreatureResponse> ExecuteAsync(CreateCreatureRequest request)
    {
        if (!Enum.TryParse<CreatureType>(request.Type, out var type))
            throw new ArgumentException("Le type doit être Tank, Healer ou Attacker.");

        var creature = new Creature(
            Guid.NewGuid(), request.Name, type,
            request.MaxHp, request.Attack, request.Defense, request.SpecialPower);

        await _creatureRepository.AddAsync(creature);
        return CreatureResponse.FromDomain(creature);
    }
}
