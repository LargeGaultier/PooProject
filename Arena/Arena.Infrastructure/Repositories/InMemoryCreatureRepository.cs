using Arena.Domain.Entities;
using Arena.Domain.Repositories;

namespace Arena.Infrastructure.Repositories;

public class InMemoryCreatureRepository : ICreatureRepository
{
    private readonly List<Creature> _creatures = new();

    public Task<List<Creature>> GetAllAsync()
        => Task.FromResult(_creatures.Select(Clone).ToList());

    public Task<Creature?> GetByIdAsync(Guid id)
    {
        var creature = _creatures.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(creature is null ? null : Clone(creature));
    }

    public Task AddAsync(Creature creature)
    {
        _creatures.Add(creature);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var creature = _creatures.FirstOrDefault(c => c.Id == id);
        if (creature is null) return Task.FromResult(false);
        _creatures.Remove(creature);
        return Task.FromResult(true);
    }

    // Clone pour éviter que les mutations du combat affectent les données stockées
    private static Creature Clone(Creature c) =>
        new(c.Id, c.Name, c.Type, c.MaxHp, c.Attack, c.Defense, c.SpecialPower);
}
