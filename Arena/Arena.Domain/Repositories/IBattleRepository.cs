using Arena.Domain.Entities;

namespace Arena.Domain.Repositories;

public interface IBattleRepository
{
    Task<List<Battle>> GetAllAsync();
    Task<Battle?> GetByIdAsync(Guid id);
    Task AddAsync(Battle battle);
}
