namespace ArenaLite.Models;

/// <summary>
/// Classe de base abstraite pour toutes les créatures de combat.
/// Contient l'état commun et les opérations partagées.
/// </summary>
public abstract class Fighter
{
    protected static readonly Random Rng = new();

    private int hp;
    private int poisonTurns;

    public string Name { get; }
    public FighterType Type { get; }
    public int MaxHp { get; }
    public int Atk { get; }
    public int Special { get; }
    public int Hp => hp;
    public int PoisonTurns => poisonTurns;
    public bool IsAlive => hp > 0;

    protected Fighter(string name, FighterType type)
    {
        Name = name;
        Type = type;
        MaxHp = type.BaseMaxHp;
        hp = MaxHp;
        Atk = type.BaseAtk;
        Special = type.BaseSpecial;
    }

    /// <summary>
    /// Exécute l'attaque spéciale sur la cible.
    /// Retourne le message décrivant l'action (pour l'affichage).
    /// </summary>
    public abstract string UseSpecial(Fighter target);

    // ---------- Opérations sur l'état ----------

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp < 0) hp = 0;
    }

    public void Heal(int amount)
    {
        hp += amount;
        if (hp > MaxHp) hp = MaxHp;
    }

    public void SetPoison(int turns)
    {
        poisonTurns = turns;
    }

    public void TickPoison()
    {
        if (poisonTurns > 0) poisonTurns--;
    }

    // ---------- Utilitaire protégé pour les sous-classes ----------

    protected double GetTypeBonusAgainst(Fighter target)
    {
        return FighterType.GetTypeBonus(Type, target.Type);
    }
}
