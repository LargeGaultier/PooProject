using ArenaLite.Models.Fighters;

namespace ArenaLite.Models;

/// <summary>
/// Factory — Centralise la création des créatures.
/// L'ajout d'un nouveau type = un Register, pas de switch ailleurs.
/// </summary>
public class FighterFactory
{
    private readonly Dictionary<FighterType, Func<string, Fighter>> creators = new();

    public FighterFactory()
    {
        Register(FighterType.Fire,     name => new FireFighter(name));
        Register(FighterType.Water,    name => new WaterFighter(name));
        Register(FighterType.Grass,    name => new GrassFighter(name));
        Register(FighterType.Electric, name => new ElectricFighter(name));
        Register(FighterType.Ice,      name => new IceFighter(name));
    }

    public void Register(FighterType type, Func<string, Fighter> creator)
    {
        creators[type] = creator;
    }

    public Fighter Create(FighterType type, string name)
    {
        if (!creators.TryGetValue(type, out var creator))
            throw new ArgumentException($"Type inconnu : {type.Name}");
        return creator(name);
    }
}
