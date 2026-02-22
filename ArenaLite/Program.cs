// ============================================================
// ArenaLite — Simulateur de combat au tour par tour (procédural)
// Code VOLONTAIREMENT procédural et "pas propre" pour servir
// de base à une refactorisation vers la POO.
// ============================================================

// ---------- Données globales (tableaux parallèles) ----------
// Index 0 = Fire, 1 = Water, 2 = Grass
string[] typeNames = { "Fire", "Water", "Grass" };
int[] baseMaxHP =   {  100,    120,     110  };
int[] baseAtk =     {   25,     18,      20  };
int[] baseSpecial = {   40,     30,      15  }; // Fire=gros dégâts, Water=soin, Grass=poison

// Combattants : on stocke tout dans des variables plates
string name1 = "", name2 = "";
int type1 = 0, type2 = 0;
int maxHp1 = 0, maxHp2 = 0;
int hp1 = 0, hp2 = 0;
int atk1 = 0, atk2 = 0;
int special1 = 0, special2 = 0;
int poisonTurns1 = 0, poisonTurns2 = 0; // tours de poison restants

Random rng = new Random();

// ====================== MAIN ======================
Console.WriteLine("=== ARENA LITE — Combat au tour par tour ===\n");

Console.WriteLine("--- Joueur 1, choisis ta créature ---");
ChooseFighter(out name1, out type1, out maxHp1, out hp1, out atk1, out special1);

Console.WriteLine("\n--- Joueur 2, choisis ta créature ---");
ChooseFighter(out name2, out type2, out maxHp2, out hp2, out atk2, out special2);

Console.WriteLine($"\n>> {name1} ({typeNames[type1]}) VS {name2} ({typeNames[type2]}) — FIGHT!\n");

int turn = 0;
while (hp1 > 0 && hp2 > 0)
{
    bool isPlayer1 = (turn % 2 == 0);
    string atkName = isPlayer1 ? name1 : name2;
    string defName = isPlayer1 ? name2 : name1;

    Console.WriteLine($"-- Tour de {atkName} --");
    int choice = ReadChoice("Action ? 1) Attack  2) Special : ", 1, 2);

    if (choice == 1)
    {
        if (isPlayer1)
            DoAttack(name1, type1, atk1, name2, type2, ref hp2);
        else
            DoAttack(name2, type2, atk2, name1, type1, ref hp1);
    }
    else
    {
        if (isPlayer1)
            DoSpecial(name1, type1, special1, ref hp1, maxHp1, name2, type2, ref hp2, ref poisonTurns2);
        else
            DoSpecial(name2, type2, special2, ref hp2, maxHp2, name1, type1, ref hp1, ref poisonTurns1);
    }

    // Appliquer le poison en fin de tour du défenseur
    if (isPlayer1 && poisonTurns2 > 0)
    {
        hp2 -= 8;
        hp2 = ClampHP(hp2, maxHp2);
        poisonTurns2--;
        Console.WriteLine($"  [Poison] {name2} perd 8 HP ! ({poisonTurns2} tours restants)");
    }
    else if (!isPlayer1 && poisonTurns1 > 0)
    {
        hp1 -= 8;
        hp1 = ClampHP(hp1, maxHp1);
        poisonTurns1--;
        Console.WriteLine($"  [Poison] {name1} perd 8 HP ! ({poisonTurns1} tours restants)");
    }

    PrintStatus(name1, hp1, maxHp1, name2, hp2, maxHp2);
    turn++;
}

string winner = hp1 > 0 ? name1 : name2;
Console.WriteLine($"\n*** {winner} remporte le combat ! ***");

// =================== FONCTIONS ===================

void ChooseFighter(out string name, out int type, out int maxHp, out int hp, out int atk, out int special)
{
    Console.WriteLine("  1) Fire   — ATK élevée, Spécial = gros dégâts");
    Console.WriteLine("  2) Water  — HP élevés,  Spécial = soin");
    Console.WriteLine("  3) Grass  — Équilibré,  Spécial = poison");
    int c = ReadChoice("  Ton choix (1-3) : ", 1, 3);
    type = c - 1; // 0=Fire, 1=Water, 2=Grass

    Console.Write("  Nom de ta créature : ");
    name = Console.ReadLine() ?? typeNames[type];
    if (string.IsNullOrWhiteSpace(name)) name = typeNames[type];

    maxHp = baseMaxHP[type];
    hp = maxHp;
    atk = baseAtk[type];
    special = baseSpecial[type];
}

int ReadChoice(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int val) && val >= min && val <= max)
            return val;
        Console.WriteLine($"  Entrée invalide, choisis entre {min} et {max}.");
    }
}

void DoAttack(string atkName, int atkType, int atkPower, string defName, int defType, ref int defHp)
{
    int damage = atkPower + rng.Next(-3, 4); // petite variation
    double mult = TypeBonus(atkType, defType);
    damage = (int)(damage * mult);
    if (damage < 1) damage = 1;
    defHp -= damage;
    defHp = ClampHP(defHp, 9999);

    string bonusText = mult > 1.0 ? " (super efficace!)" : mult < 1.0 ? " (pas très efficace...)" : "";
    Console.WriteLine($"  {atkName} attaque {defName} pour {damage} dégâts !{bonusText}");
}

void DoSpecial(string atkName, int atkType, int specPower,
               ref int atkHp, int atkMaxHp,
               string defName, int defType, ref int defHp,
               ref int defPoisonTurns)
{
    if (atkType == 0) // Fire — gros dégâts
    {
        int damage = specPower + rng.Next(-5, 6);
        double mult = TypeBonus(atkType, defType);
        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;
        defHp -= damage;
        defHp = ClampHP(defHp, 9999);
        Console.WriteLine($"  {atkName} lance Flamme Intense sur {defName} pour {damage} dégâts !");
    }
    else if (atkType == 1) // Water — soin
    {
        int heal = specPower + rng.Next(-3, 4);
        atkHp += heal;
        atkHp = ClampHP(atkHp, atkMaxHp);
        Console.WriteLine($"  {atkName} utilise Vague Curative et récupère {heal} HP !");
    }
    else // Grass — poison (3 tours, 8 dégâts/tour)
    {
        defPoisonTurns = 3;
        Console.WriteLine($"  {atkName} empoisonne {defName} pour 3 tours !");
    }
}

double TypeBonus(int atkType, int defType)
{
    // Fire > Grass, Grass > Water, Water > Fire
    if ((atkType == 0 && defType == 2) || // Fire > Grass
        (atkType == 2 && defType == 1) || // Grass > Water
        (atkType == 1 && defType == 0))   // Water > Fire
        return 1.25;

    if ((atkType == 0 && defType == 1) || // Fire < Water
        (atkType == 2 && defType == 0) || // Grass < Fire
        (atkType == 1 && defType == 2))   // Water < Grass
        return 0.75;

    return 1.0; // même type
}

int ClampHP(int hp, int maxHp)
{
    if (hp < 0) return 0;
    if (hp > maxHp) return maxHp;
    return hp;
}

void PrintStatus(string n1, int h1, int m1, string n2, int h2, int m2)
{
    Console.WriteLine($"  [ {n1}: {h1}/{m1} HP | {n2}: {h2}/{m2} HP ]\n");
}
