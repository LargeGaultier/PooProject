using Arena.Business.DTOs;
using Arena.DataAccess.Entities;
using Arena.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arena.Business;

public class CreatureService
{
    private readonly CreatureRepository _creatureRepository;

    public CreatureService(CreatureRepository creatureRepository)
    {
        _creatureRepository = creatureRepository;
    }

    public async Task<List<CreatureResponse>> GetAllAsync()
    {
        return await _creatureRepository.Query()
            .Select(CreatureResponse.Projection)
            .ToListAsync();
    }

    public async Task<CreatureResponse?> GetByIdAsync(Guid id)
    {
        return await _creatureRepository.Query()
            .Where(c => c.Id == id)
            .Select(CreatureResponse.Projection)
            .FirstOrDefaultAsync();
    }

    public async Task<CreatureResponse> CreateAsync(CreateCreatureRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Le nom de la créature ne peut pas être vide.");

        if (request.MaxHp <= 0)
            throw new ArgumentException("Les HP doivent être supérieurs à 0.");

        if (request.Attack < 0)
            throw new ArgumentException("L'attaque ne peut pas être négative.");

        if (request.Defense < 0)
            throw new ArgumentException("La défense ne peut pas être négative.");

        var validTypes = new[] { "Tank", "Healer", "Attacker" };
        if (!validTypes.Contains(request.Type))
            throw new ArgumentException("Le type doit être Tank, Healer ou Attacker.");

        var entity = new CreatureEntity
        {
            Name = request.Name,
            Type = request.Type,
            MaxHp = request.MaxHp,
            Attack = request.Attack,
            Defense = request.Defense,
            SpecialPower = request.SpecialPower
        };

        var created = await _creatureRepository.AddAsync(entity);

        return await _creatureRepository.Query()
            .Where(c => c.Id == created.Id)
            .Select(CreatureResponse.Projection)
            .FirstAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _creatureRepository.DeleteAsync(id);
    }
}
