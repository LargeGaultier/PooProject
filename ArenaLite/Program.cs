using ArenaLite.Models;
using ArenaLite.Controllers;
using ArenaLite.Views;
using ArenaLite.Observers;
using ArenaLite.Logging;

// ============================================================
// ArenaLite — Composition root
// Assemble les dépendances, branche les patterns, lance le jeu
// ============================================================

Console.WriteLine("=== ARENA LITE — Combat au tour par tour ===\n");

// --- 1. Singleton : initialisation du logger ---
ILogger logger = ChooseLogger();
LoggerProvider.Initialize(logger);

// --- 2. View + Factory ---
GameView view = new GameView();
FighterFactory factory = new FighterFactory();

// --- 3. Choix des créatures (via Factory) ---
Console.WriteLine();
view.ShowPlayerPrompt(1);
Fighter fighter1 = ChooseFighter();

Console.WriteLine();
view.ShowPlayerPrompt(2);
Fighter fighter2 = ChooseFighter();

// --- 4. Controller + Observers ---
GameRules rules = new GameRules();
GameController controller = new GameController(fighter1, fighter2, rules, view);

CombatLogger combatLogger = new CombatLogger();
ScoreTracker scoreTracker = new ScoreTracker();
BattleHistory history = new BattleHistory();

controller.AddObserver(combatLogger);
controller.AddObserver(scoreTracker);
controller.AddObserver(history);

// --- 5. Lancement ---
controller.Run();

// --- 6. Post-combat ---
scoreTracker.PrintSummary();

// =================== FONCTIONS ===================

Fighter ChooseFighter()
{
    int choice = view.AskFighterType();
    FighterType type = FighterType.All[choice - 1];
    string name = view.AskFighterName(type.Name);
    return factory.Create(type, name);
}

ILogger ChooseLogger()
{
    Console.WriteLine("Mode de log :");
    Console.WriteLine("  1) Console    — affichage normal");
    Console.WriteLine("  2) Fichier    — écriture dans un fichier");
    Console.WriteLine("  3) Silencieux — aucun affichage de combat");
    int choice = ReadChoice("  Ton choix (1-3) : ", 1, 3);

    return choice switch
    {
        1 => new ConsoleLogger(),
        2 => CreateFileLogger(),
        3 => new SilentLogger(),
        _ => new ConsoleLogger()
    };
}

ILogger CreateFileLogger()
{
    Console.Write("  Nom du fichier (défaut: combat.log) : ");
    string? path = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(path)) path = "combat.log";
    return new FileLogger(path);
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
