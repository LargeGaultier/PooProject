// Version pédagogique volontairement mal conçue (anti-patterns)
class GameManager
{
    public static Random Rng = new Random();
    public static List<string> BattleLog = new List<string>();
    public static int TurnCount = 0;

    private ILogger _logger = new ConsoleLogger();
    private Creature _fighter1 = new Creature();
    private Creature _fighter2 = new Creature();

    public void Run()
    {
        TurnCount = 0;
        BattleLog.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("   ARENA LITE - Combat tour par tour");
        Console.WriteLine("========================================\n");

        _logger.Log("Demarrage du jeu...");

        Console.WriteLine("--- Joueur 1, choisis ta creature ---");
        _fighter1 = CreateFighter();

        Console.WriteLine("\n--- Joueur 2, choisis ta creature ---");
        _fighter2 = CreateFighter();

        Console.WriteLine();
        Utils.PrintSeparator();
        Console.WriteLine($"  {_fighter1.Name} ({Utils.GetTypeName(_fighter1.Type)}) VS {_fighter2.Name} ({Utils.GetTypeName(_fighter2.Type)})");
        Utils.PrintSeparator();
        Console.WriteLine();

        BattleLog.Add($"Combat: {_fighter1.Name} vs {_fighter2.Name}");

        while (_fighter1.HP > 0 && _fighter2.HP > 0)
        {
            bool isPlayer1Turn = (TurnCount % 2 == 0);
            Creature attacker = isPlayer1Turn ? _fighter1 : _fighter2;
            Creature defender = isPlayer1Turn ? _fighter2 : _fighter1;

            Console.WriteLine($"-- Tour {TurnCount + 1} : {attacker.Name} --");

            Console.Write("  Action ? 1) Attack  2) Special : ");
            string? input = Console.ReadLine();
            int choice = 0;
            while (!int.TryParse(input, out choice) || choice < 1 || choice > 2)
            {
                Console.Write("  Invalide. 1) Attack  2) Special : ");
                input = Console.ReadLine();
            }

            if (choice == 1)
            {
                DoAttack(attacker, defender);
            }
            else
            {
                DoSpecial(attacker, defender);
            }

            if (defender.PoisonTurns > 0)
            {
                int poisonDmg = 8;
                defender.HP -= poisonDmg;
                if (defender.HP < 0) defender.HP = 0;
                defender.PoisonTurns--;
                Console.WriteLine($"  [Poison] {defender.Name} perd {poisonDmg} HP ! ({defender.PoisonTurns} tours restants)");
                BattleLog.Add($"T{TurnCount + 1}: Poison -> {defender.Name} (-{poisonDmg})");
            }

            Console.WriteLine($"  [ {_fighter1.Name}: {_fighter1.HP}/{_fighter1.MaxHP} HP | {_fighter2.Name}: {_fighter2.HP}/{_fighter2.MaxHP} HP ]");
            Console.WriteLine();

            TurnCount++;
        }

        Utils.PrintSeparator();
        string winner = _fighter1.HP > 0 ? _fighter1.Name : _fighter2.Name;
        Console.WriteLine($"\n  *** {winner} remporte le combat ! ***\n");
        BattleLog.Add($"Vainqueur: {winner} en {TurnCount} tours");
        _logger.Log("Combat termine.");

        Utils.PrintBattleLog();
    }

    private Creature CreateFighter()
    {
        Console.WriteLine("  1) Fire   - ATK elevee, Special = gros degats");
        Console.WriteLine("  2) Water  - HP eleves,  Special = soin");
        Console.WriteLine("  3) Grass  - Equilibre,  Special = poison");

        Console.Write("  Ton choix (1-3) : ");
        string? input = Console.ReadLine();
        int choice = 0;
        while (!int.TryParse(input, out choice) || choice < 1 || choice > 3)
        {
            Console.Write("  Invalide, choisis 1-3 : ");
            input = Console.ReadLine();
        }

        Creature c = new Creature();

        switch (choice)
        {
            case 1:
                c.Type = 0;
                c.MaxHP = 100;
                c.HP = 100;
                c.AttackPower = 25;
                c.SpecialPower = 40;
                break;
            case 2:
                c.Type = 1;
                c.MaxHP = 120;
                c.HP = 120;
                c.AttackPower = 18;
                c.SpecialPower = 30;
                break;
            case 3:
                c.Type = 2;
                c.MaxHP = 110;
                c.HP = 110;
                c.AttackPower = 20;
                c.SpecialPower = 15;
                break;
        }

        Console.Write("  Nom de ta creature : ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) name = Utils.GetTypeName(c.Type);
        c.Name = name;

        _logger.Log($"Creature creee: {c.Name} ({Utils.GetTypeName(c.Type)})");
        return c;
    }

    private void DoAttack(Creature atk, Creature def)
    {
        int damage = atk.AttackPower + Rng.Next(-3, 4);

        double mult = 1.0;
        if ((atk.Type == 0 && def.Type == 2) ||
            (atk.Type == 2 && def.Type == 1) ||
            (atk.Type == 1 && def.Type == 0))
        {
            mult = 1.25;
        }
        else if ((atk.Type == 0 && def.Type == 1) ||
                 (atk.Type == 2 && def.Type == 0) ||
                 (atk.Type == 1 && def.Type == 2))
        {
            mult = 0.75;
        }

        damage = (int)(damage * mult);
        if (damage < 1) damage = 1;
        def.HP -= damage;
        if (def.HP < 0) def.HP = 0;

        string bonusText = mult > 1.0 ? " (super efficace!)" : mult < 1.0 ? " (pas tres efficace...)" : "";
        Console.WriteLine($"  {atk.Name} attaque {def.Name} pour {damage} degats !{bonusText}");
        BattleLog.Add($"T{TurnCount + 1}: {atk.Name} -> ATK -> {def.Name} ({damage})");
    }

    private void DoSpecial(Creature atk, Creature def)
    {
        if (atk.Type == 0)
        {
            int damage = atk.SpecialPower + Rng.Next(-5, 6);

            double mult = 1.0;
            if ((atk.Type == 0 && def.Type == 2) ||
                (atk.Type == 2 && def.Type == 1) ||
                (atk.Type == 1 && def.Type == 0))
            {
                mult = 1.25;
            }
            else if ((atk.Type == 0 && def.Type == 1) ||
                     (atk.Type == 2 && def.Type == 0) ||
                     (atk.Type == 1 && def.Type == 2))
            {
                mult = 0.75;
            }

            damage = (int)(damage * mult);
            if (damage < 1) damage = 1;
            def.HP -= damage;
            if (def.HP < 0) def.HP = 0;

            Console.WriteLine($"  {atk.Name} lance Flamme Intense sur {def.Name} pour {damage} degats !");
            BattleLog.Add($"T{TurnCount + 1}: {atk.Name} -> FIRE SPE -> {def.Name} ({damage})");
        }
        else if (atk.Type == 1)
        {
            int heal = atk.SpecialPower + Rng.Next(-3, 4);
            atk.HP += heal;
            if (atk.HP > atk.MaxHP) atk.HP = atk.MaxHP;

            Console.WriteLine($"  {atk.Name} utilise Vague Curative et recupere {heal} HP !");
            BattleLog.Add($"T{TurnCount + 1}: {atk.Name} -> WATER SPE -> SELF (+{heal})");
        }
        else if (atk.Type == 2)
        {
            def.PoisonTurns = 3;

            Console.WriteLine($"  {atk.Name} empoisonne {def.Name} pour 3 tours !");
            BattleLog.Add($"T{TurnCount + 1}: {atk.Name} -> GRASS SPE -> {def.Name} (poison 3t)");
        }
    }

    public void DisplayFighterDetails(Creature c)
    {
        Console.WriteLine($"  {c.Name} [{Utils.GetTypeName(c.Type)}] : {c.HP}/{c.MaxHP} HP, ATK={c.AttackPower}, SPE={c.SpecialPower}");
    }
}
