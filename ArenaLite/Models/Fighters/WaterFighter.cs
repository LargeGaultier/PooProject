namespace ArenaLite.Models.Fighters;

/// <summary>
/// Créature de type Water — Spécial : Vague Curative (soin).
/// </summary>
public class WaterFighter : Fighter
{
    public WaterFighter(string name) : base(name, FighterType.Water) { }

    public override string UseSpecial(Fighter target)
    {
        int heal = Special + Rng.Next(-3, 4);
        Heal(heal);
        return $"  {Name} utilise Vague Curative et récupère {heal} HP !";
    }
}
