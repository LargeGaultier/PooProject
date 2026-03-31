using Arena.Domain.Entities;
using Arena.Domain.Repositories;

namespace Arena.Infrastructure.Repositories;

public class InMemoryBattleRepository : IBattleRepository
{
    private readonly List<Battle> _battles = new();

    public Task<List<Battle>> GetAllAsync()
        => Task.FromResult(_battles.ToList());

    public Task<Battle?> GetByIdAsync(Guid id)
        => Task.FromResult(_battles.FirstOrDefault(b => b.Id == id));

    public Task AddAsync(Battle battle)
    {
        _battles.Add(battle);
        return Task.CompletedTask;
    }
}
