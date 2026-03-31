using Arena.Domain.Entities;

namespace Arena.Domain.Repositories;

public interface ICreatureRepository
{
    Task<List<Creature>> GetAllAsync();
    Task<Creature?> GetByIdAsync(Guid id);
    Task AddAsync(Creature creature);
    Task<bool> DeleteAsync(Guid id);
}
