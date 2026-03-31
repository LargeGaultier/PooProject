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

    public IQueryable<BattleEntity> Query()
    {
        return _context.Battles.AsQueryable();
    }

    public async Task<BattleEntity> AddAsync(BattleEntity battle)
    {
        battle.Id = Guid.NewGuid();
        _context.Battles.Add(battle);
        await _context.SaveChangesAsync();
        return battle;
    }
}
