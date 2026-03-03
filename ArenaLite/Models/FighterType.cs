namespace ArenaLite.Models;

/// <summary>
/// Décrit un type de créature avec ses statistiques de base.
/// </summary>
public class FighterType
{
    public string Name { get; }
    public int BaseMaxHp { get; }
    public int BaseAtk { get; }
    public int BaseSpecial { get; }
    public string SpecialDescription { get; }

    private FighterType(string name, int baseMaxHp, int baseAtk, int baseSpecial, string specialDescription)
    {
        Name = name;
        BaseMaxHp = baseMaxHp;
        BaseAtk = baseAtk;
        BaseSpecial = baseSpecial;
        SpecialDescription = specialDescription;
    }

    // ---------- Types disponibles ----------
    public static readonly FighterType Fire  = new("Fire",  100, 25, 40, "gros dégâts");
    public static readonly FighterType Water = new("Water", 120, 18, 30, "soin");
    public static readonly FighterType Grass = new("Grass", 110, 20, 15, "poison");

    public static readonly FighterType[] All = { Fire, Water, Grass };
}
