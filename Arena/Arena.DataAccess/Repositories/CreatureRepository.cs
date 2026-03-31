using Arena.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arena.DataAccess.Repositories;

public class CreatureRepository
{
    private readonly ArenaDbContext _context;

    public CreatureRepository(ArenaDbContext context)
    {
        _context = context;
    }

    public IQueryable<CreatureEntity> Query()
    {
        return _context.Creatures.AsQueryable();
    }

    public async Task<CreatureEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Creatures.FindAsync(id);
    }

    public async Task<CreatureEntity> AddAsync(CreatureEntity creature)
    {
        creature.Id = Guid.NewGuid();
        _context.Creatures.Add(creature);
        await _context.SaveChangesAsync();
        return creature;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var creature = await _context.Creatures.FindAsync(id);
        if (creature == null) return false;

        _context.Creatures.Remove(creature);
        await _context.SaveChangesAsync();
        return true;
    }
}
