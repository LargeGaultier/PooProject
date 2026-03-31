using Arena.Application.DTOs;
using Arena.Domain;
using Arena.Domain.Entities;
using Arena.Domain.Repositories;

namespace Arena.Application.Services;

public class CreatureAppService
{
    private readonly ICreatureRepository _creatureRepository;

    public CreatureAppService(ICreatureRepository creatureRepository)
    {
        _creatureRepository = creatureRepository;
    }

    public async Task<List<CreatureResponse>> GetAllAsync()
    {
        var creatures = await _creatureRepository.GetAllAsync();
        return creatures.Select(ToResponse).ToList();
    }

    public async Task<CreatureResponse?> GetByIdAsync(Guid id)
    {
        var creature = await _creatureRepository.GetByIdAsync(id);
        return creature is null ? null : ToResponse(creature);
    }

    public async Task<CreatureResponse> CreateAsync(CreateCreatureRequest request)
    {
        if (!Enum.TryParse<CreatureType>(request.Type, out var type))
            throw new ArgumentException("Le type doit être Tank, Healer ou Attacker.");

        var creature = new Creature(
            Guid.NewGuid(), request.Name, type,
            request.MaxHp, request.Attack, request.Defense, request.SpecialPower);

        await _creatureRepository.AddAsync(creature);
        return ToResponse(creature);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _creatureRepository.DeleteAsync(id);
    }

    private static CreatureResponse ToResponse(Creature c) =>
        new(c.Id, c.Name, c.Type.ToString(), c.MaxHp, c.Attack, c.Defense, c.SpecialPower);
}
