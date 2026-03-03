using ArenaLite.Models;

namespace ArenaLite.Core;

/// <summary>
/// Règles métier communes du combat : attaque normale, poison, bonus de type.
/// Les spéciaux sont gérés par chaque sous-classe de Fighter (polymorphisme).
/// </summary>
public class GameRules
{
    private readonly Random rng = new();

    public const int PoisonDamagePerTurn = 8;

    // ---------- Attaque normale ----------

    public (int damage, double multiplier) DoAttack(Fighter attacker, Fighter defender)
    {
        int damage = attacker.Atk + rng.Next(-3, 4);
        double mult = FighterType.GetTypeBonus(attacker.Type, defender.Type);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;
        defender.TakeDamage(damage);
        return (damage, mult);
    }

    // ---------- Poison ----------

    public bool ApplyPoison(Fighter fighter)
    {
        if (fighter.PoisonTurns <= 0) return false;
        fighter.TakeDamage(PoisonDamagePerTurn);
        fighter.TickPoison();
        return true;
    }
}
