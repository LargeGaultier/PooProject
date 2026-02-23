// Version pédagogique volontairement mal conçue (anti-patterns)
static class Utils
{
    public static string GetTypeName(int type)
    {
        if (type == 0) return "Fire";
        if (type == 1) return "Water";
        if (type == 2) return "Grass";
        return "???";
    }

    public static void PrintSeparator()
    {
        Console.WriteLine("================================");
    }

    public static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static void PrintBattleLog()
    {
        Console.WriteLine("\n=== Journal de combat ===");
        for (int i = 0; i < GameManager.BattleLog.Count; i++)
        {
            Console.WriteLine("  " + GameManager.BattleLog[i]);
        }
    }

    public static int ApplyTypeModifier(int damage, int atkType, int defType)
    {
        if ((atkType == 0 && defType == 2) ||
            (atkType == 2 && defType == 1) ||
            (atkType == 1 && defType == 0))
        {
            damage = (int)(damage * 1.25);
        }
        else if ((atkType == 0 && defType == 1) ||
                 (atkType == 2 && defType == 0) ||
                 (atkType == 1 && defType == 2))
        {
            damage = (int)(damage * 0.75);
        }
        return damage;
    }

    public static int RandomDamageVariation(int baseDamage, int range)
    {
        return baseDamage + GameManager.Rng.Next(-range, range + 1);
    }
}
