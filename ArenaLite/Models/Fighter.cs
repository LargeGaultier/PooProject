namespace ArenaLite.Models;

/// <summary>
/// Une créature de combat avec ses caractéristiques et son état.
/// </summary>
public class Fighter
{
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

    public Fighter(string name, FighterType type)
    {
        Name = name;
        Type = type;
        MaxHp = type.BaseMaxHp;
        hp = MaxHp;
        Atk = type.BaseAtk;
        Special = type.BaseSpecial;
    }

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
}
