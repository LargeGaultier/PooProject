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
);
