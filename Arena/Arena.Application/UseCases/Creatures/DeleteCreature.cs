using Arena.Domain.Repositories;

namespace Arena.Application.UseCases.Creatures;

public class DeleteCreature
{
    private readonly ICreatureRepository _creatureRepository;

    public DeleteCreature(ICreatureRepository creatureRepository)
    {
        _creatureRepository = creatureRepository;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        return await _creatureRepository.DeleteAsync(id);
    }
}
