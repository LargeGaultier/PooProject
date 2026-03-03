namespace ArenaLite.Models.Fighters;

/// <summary>
/// Créature de type Electric — Spécial : Éclair Foudroyant (dégâts avec bonus de type).
/// Ajoutée en Partie 4 pour valider l'extensibilité du design.
/// </summary>
public class ElectricFighter : Fighter
{
    public ElectricFighter(string name) : base(name, FighterType.Electric) { }

    public override string UseSpecial(Fighter target)
    {
        int damage = Special + Rng.Next(-4, 5);
        double mult = GetTypeBonusAgainst(target);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;

        target.TakeDamage(damage);
        return $"  {Name} lance Éclair Foudroyant sur {target.Name} pour {damage} dégâts !";
    }
}
