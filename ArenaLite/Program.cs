using ArenaLite.Models;
using ArenaLite.Models.Fighters;
using ArenaLite.Core;
using ArenaLite.Logging;
using ArenaLite.UI;

// ============================================================
// ArenaLite — Point d'entrée
// Assemblage des dépendances et lancement du jeu
// ============================================================

Console.WriteLine("=== ARENA LITE — Combat au tour par tour ===\n");

// --- 1. Choix du mode de log ---
ILogger logger = ChooseLogger();

// --- 2. Assemblage des dépendances ---
GameInterface ui = new GameInterface(logger);
GameRules rules = new GameRules();

// --- 3. Choix des créatures (interactif) ---
Console.WriteLine();
ui.ShowPlayerPrompt(1);
Fighter fighter1 = ChooseFighter();

Console.WriteLine();
ui.ShowPlayerPrompt(2);
Fighter fighter2 = ChooseFighter();

// --- 4. Lancement du combat ---
Game game = new Game(fighter1, fighter2, rules, ui);
game.Run();

// =================== FONCTIONS ===================

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

Fighter ChooseFighter()
{
    int choice = ui.AskFighterType();
    FighterType type = FighterType.All[choice - 1];
    string name = ui.AskFighterName(type.Name);

    return choice switch
    {
        1 => new FireFighter(name),
        2 => new WaterFighter(name),
        3 => new GrassFighter(name),
        4 => new ElectricFighter(name),
        _ => throw new InvalidOperationException("Type inconnu")
    };
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
