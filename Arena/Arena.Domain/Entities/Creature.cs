namespace Arena.Domain.Entities;

public class Creature
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CreatureType Type { get; private set; }
    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public string SpecialPower { get; private set; }

    public bool IsAlive => CurrentHp > 0;

    public Creature(Guid id, string name, CreatureType type, int maxHp, int attack, int defense, string specialPower)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom de la créature ne peut pas être vide.");
        if (maxHp <= 0)
            throw new DomainException("Les HP doivent être supérieurs à 0.");
        if (attack < 0)
            throw new DomainException("L'attaque ne peut pas être négative.");
        if (defense < 0)
            throw new DomainException("La défense ne peut pas être négative.");

        Id = id;
        Name = name;
        Type = type;
        MaxHp = maxHp;
        CurrentHp = maxHp;
        Attack = attack;
        Defense = defense;
        SpecialPower = specialPower;
    }

    public int TakeDamage(int rawDamage)
    {
        var actualDamage = Math.Max(1, rawDamage - Defense);
        CurrentHp = Math.Max(0, CurrentHp - actualDamage);
        return actualDamage;
    }
}
