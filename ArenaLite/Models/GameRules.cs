namespace ArenaLite.Models;

/// <summary>
/// MVC Model — Règles métier communes du combat.
/// Aucune dépendance vers la console, la vue ou les observers.
/// </summary>
public class GameRules
{
    private readonly Random rng = new();

    public const int PoisonDamagePerTurn = 8;

    public (int damage, double multiplier) DoAttack(Fighter attacker, Fighter defender)
    {
        int damage = attacker.Atk + rng.Next(-3, 4);
        double mult = FighterType.GetTypeBonus(attacker.Type, defender.Type);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;
        defender.TakeDamage(damage);
        return (damage, mult);
    }

    public bool ApplyPoison(Fighter fighter)
    {
        if (fighter.PoisonTurns <= 0) return false;
        fighter.TakeDamage(PoisonDamagePerTurn);
        fighter.TickPoison();
        return true;
    }
}
