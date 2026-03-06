namespace ArenaLite.Models.Fighters;

/// <summary>
/// Créature de type Ice — Spécial : Blizzard (dégâts avec bonus de type).
/// Ajoutée en Partie 5 pour valider l'extensibilité.
/// </summary>
public class IceFighter : Fighter
{
    public IceFighter(string name) : base(name, FighterType.Ice) { }

    public override string UseSpecial(Fighter target)
    {
        int damage = Special + Rng.Next(-4, 5);
        double mult = GetTypeBonusAgainst(target);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;

        target.TakeDamage(damage);
        return $"  {Name} lance Blizzard sur {target.Name} pour {damage} dégâts !";
    }
}
