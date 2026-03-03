using ArenaLite.Models;

namespace ArenaLite.Core;

/// <summary>
/// Règles métier du combat : calculs de dégâts, bonus de type, effets des spéciaux.
/// </summary>
public class GameRules
{
    private readonly Random rng = new();

    public const int PoisonDamagePerTurn = 8;
    public const int PoisonDuration = 3;

    // ---------- Attaque normale ----------

    public int DoAttack(Fighter attacker, Fighter defender)
    {
        int damage = attacker.Atk + rng.Next(-3, 4);
        double mult = GetTypeBonus(attacker.Type, defender.Type);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;
        defender.TakeDamage(damage);
        return damage;
    }

    // ---------- Spéciaux par type ----------

    public int DoSpecialFire(Fighter attacker, Fighter defender)
    {
        int damage = attacker.Special + rng.Next(-5, 6);
        double mult = GetTypeBonus(attacker.Type, defender.Type);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;
        defender.TakeDamage(damage);
        return damage;
    }

    public int DoSpecialWater(Fighter attacker)
    {
        int heal = attacker.Special + rng.Next(-3, 4);
        attacker.Heal(heal);
        return heal;
    }

    public void DoSpecialGrass(Fighter defender)
    {
        defender.SetPoison(PoisonDuration);
    }

    // ---------- Poison ----------

    public bool ApplyPoison(Fighter fighter)
    {
        if (fighter.PoisonTurns <= 0) return false;
        fighter.TakeDamage(PoisonDamagePerTurn);
        fighter.TickPoison();
        return true;
    }

    // ---------- Bonus de type ----------

    public double GetTypeBonus(FighterType atkType, FighterType defType)
    {
        // Fire > Grass, Grass > Water, Water > Fire
        if ((atkType == FighterType.Fire  && defType == FighterType.Grass) ||
            (atkType == FighterType.Grass && defType == FighterType.Water) ||
            (atkType == FighterType.Water && defType == FighterType.Fire))
            return 1.25;

        if ((atkType == FighterType.Fire  && defType == FighterType.Water) ||
            (atkType == FighterType.Grass && defType == FighterType.Fire)  ||
            (atkType == FighterType.Water && defType == FighterType.Grass))
            return 0.75;

        return 1.0;
    }
}
