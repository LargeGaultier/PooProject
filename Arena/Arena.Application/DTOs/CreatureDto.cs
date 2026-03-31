using Arena.Domain.Entities;

namespace Arena.Application.DTOs;

public record CreateCreatureRequest(
    string Name,
    string Type,
    int MaxHp,
    int Attack,
    int Defense,
    string SpecialPower
);

public record CreatureResponse(
    Guid Id,
    string Name,
    string Type,
    int MaxHp,
    int Attack,
    int Defense,
    string SpecialPower
)
{
    public static CreatureResponse FromDomain(Creature c) =>
        new(c.Id, c.Name, c.Type.ToString(), c.MaxHp, c.Attack, c.Defense, c.SpecialPower);
};
