using System.Linq.Expressions;
using Arena.DataAccess.Entities;

namespace Arena.Business.DTOs;

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
    public static Expression<Func<CreatureEntity, CreatureResponse>> Projection =>
        e => new CreatureResponse(e.Id, e.Name, e.Type, e.MaxHp, e.Attack, e.Defense, e.SpecialPower);
}
