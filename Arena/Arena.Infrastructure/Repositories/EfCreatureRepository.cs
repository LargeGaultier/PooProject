using Arena.Domain;
using Arena.Domain.Entities;
using Arena.Domain.Repositories;
using Arena.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Arena.Infrastructure.Repositories;

public class EfCreatureRepository : ICreatureRepository
{
    private readonly ArenaDbContext _context;

    public EfCreatureRepository(ArenaDbContext context) => _context = context;

    public async Task<List<Creature>> GetAllAsync()
    {
        var entities = await _context.Creatures.ToListAsync();
        return entities.Select(ToDomain).ToList();
    }

    public async Task<Creature?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Creatures.FindAsync(id);
        return entity is null ? null : ToDomain(entity);
    }

    public async Task AddAsync(Creature creature)
    {
        _context.Creatures.Add(ToDbModel(creature));
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Creatures.FindAsync(id);
        if (entity is null) return false;
        _context.Creatures.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static Creature ToDomain(CreatureDbModel e) =>
        new(e.Id, e.Name, Enum.Parse<CreatureType>(e.Type), e.MaxHp, e.Attack, e.Defense, e.SpecialPower);

    private static CreatureDbModel ToDbModel(Creature c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Type = c.Type.ToString(),
        MaxHp = c.MaxHp,
        Attack = c.Attack,
        Defense = c.Defense,
        SpecialPower = c.SpecialPower
    };
}
