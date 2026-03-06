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
    public static readonly FighterType Fire     = new("Fire",     100, 25, 40, "gros dégâts");
    public static readonly FighterType Water    = new("Water",    120, 18, 30, "soin");
    public static readonly FighterType Grass    = new("Grass",    110, 20, 15, "poison");
    public static readonly FighterType Electric = new("Electric",  95, 22, 32, "éclair foudroyant");
    public static readonly FighterType Ice      = new("Ice",      105, 20, 28, "blizzard");

    public static readonly FighterType[] All = { Fire, Water, Grass, Electric, Ice };

    // ---------- Bonus de type ----------

    public static double GetTypeBonus(FighterType atkType, FighterType defType)
    {
        // Fire > Grass, Fire > Ice
        // Grass > Water, Grass > Electric
        // Water > Fire
        // Electric > Water
        // Ice > Grass
        if ((atkType == Fire     && defType == Grass)    ||
            (atkType == Fire     && defType == Ice)      ||
            (atkType == Grass    && defType == Water)    ||
            (atkType == Grass    && defType == Electric)  ||
            (atkType == Water    && defType == Fire)     ||
            (atkType == Electric && defType == Water)    ||
            (atkType == Ice      && defType == Grass))
            return 1.25;

        if ((atkType == Fire     && defType == Water)    ||
            (atkType == Grass    && defType == Fire)     ||
            (atkType == Grass    && defType == Ice)      ||
            (atkType == Water    && defType == Grass)    ||
            (atkType == Water    && defType == Electric)  ||
            (atkType == Electric && defType == Grass)    ||
            (atkType == Ice      && defType == Fire))
            return 0.75;

        return 1.0;
    }
}
