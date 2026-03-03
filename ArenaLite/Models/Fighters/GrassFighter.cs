namespace ArenaLite.Models.Fighters;

/// <summary>
/// Créature de type Grass — Spécial : Poison (3 tours, 8 dégâts/tour).
/// </summary>
public class GrassFighter : Fighter
{
    public GrassFighter(string name) : base(name, FighterType.Grass) { }

    public override string UseSpecial(Fighter target)
    {
        target.SetPoison(3);
        return $"  {Name} empoisonne {target.Name} pour 3 tours !";
    }
}
