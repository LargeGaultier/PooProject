using Arena.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arena.DataAccess.Repositories;

public class BattleRepository
{
    private readonly ArenaDbContext _context;

    public BattleRepository(ArenaDbContext context)
    {
        _context = context;
    }

    public async Task<List<BattleEntity>> GetAllAsync()
    {
        return await _context.Battles
            .Include(b => b.Creature1)
            .Include(b => b.Creature2)
            .Include(b => b.Winner)
            .Include(b => b.Logs.OrderBy(l => l.Turn))
            .ToListAsync();
    }

    public async Task<BattleEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Battles
            .Include(b => b.Creature1)
            .Include(b => b.Creature2)
            .Include(b => b.Winner)
            .Include(b => b.Logs.OrderBy(l => l.Turn))
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<BattleEntity> AddAsync(BattleEntity battle)
    {
        battle.Id = Guid.NewGuid();
        _context.Battles.Add(battle);
        await _context.SaveChangesAsync();
        return battle;
    }
}
