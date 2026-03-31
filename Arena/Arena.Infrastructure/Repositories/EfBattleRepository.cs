using Arena.Domain.Entities;
using Arena.Domain.Repositories;
using Arena.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Arena.Infrastructure.Repositories;

public class EfBattleRepository : IBattleRepository
{
    private readonly ArenaDbContext _context;

    public EfBattleRepository(ArenaDbContext context) => _context = context;

    public async Task<List<Battle>> GetAllAsync()
    {
        var entities = await _context.Battles.Include(b => b.Logs).ToListAsync();
        return entities.Select(ToDomain).ToList();
    }

    public async Task<Battle?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Battles
            .Include(b => b.Logs)
            .FirstOrDefaultAsync(b => b.Id == id);
        return entity is null ? null : ToDomain(entity);
    }

    public async Task AddAsync(Battle battle)
    {
        _context.Battles.Add(ToDbModel(battle));
        await _context.SaveChangesAsync();
    }

    private static Battle ToDomain(BattleDbModel e) =>
        new(e.Id, e.Creature1Id, e.Creature2Id, e.WinnerId, e.PlayedAt,
            e.Logs.Select(l => new BattleLog(l.Id, l.Turn, l.Description)).ToList());

    private static BattleDbModel ToDbModel(Battle b) => new()
    {
        Id = b.Id,
        Creature1Id = b.Creature1Id,
        Creature2Id = b.Creature2Id,
        WinnerId = b.WinnerId,
        PlayedAt = b.PlayedAt,
        Logs = b.Logs.Select(l => new BattleLogDbModel
        {
            Id = l.Id,
            BattleId = b.Id,
            Turn = l.Turn,
            Description = l.Description
        }).ToList()
    };
}
