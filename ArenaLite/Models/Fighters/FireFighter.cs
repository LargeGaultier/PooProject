namespace ArenaLite.Models.Fighters;

/// <summary>
/// Créature de type Fire — Spécial : Flamme Intense (gros dégâts avec bonus de type).
/// </summary>
public class FireFighter : Fighter
{
    public FireFighter(string name) : base(name, FighterType.Fire) { }

    public override string UseSpecial(Fighter target)
    {
        int damage = Special + Rng.Next(-5, 6);
        double mult = GetTypeBonusAgainst(target);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;

        target.TakeDamage(damage);
        return $"  {Name} lance Flamme Intense sur {target.Name} pour {damage} dégâts !";
    }
}
