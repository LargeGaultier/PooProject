using ArenaLite.Models;
using ArenaLite.Controllers;
using ArenaLite.Views;
using ArenaLite.Observers;
using ArenaLite.Logging;
using ArenaLite.DI;

// ============================================================
// ArenaLite — Composition root (avec container IoC maison)
// Assemble les dépendances, branche les patterns, lance le jeu
// ============================================================

Console.WriteLine("=== ARENA LITE — Combat au tour par tour ===\n");

// --- 1. Container IoC : enregistrement des dépendances ---
var container = new Container();

// Changer d'implémentation = modifier cette seule ligne :
container.Register<ILogger, ConsoleLogger>(Lifetime.Singleton);

// --- 2. Initialisation du LoggerProvider via le container ---
ILogger logger = container.Resolve<ILogger>();
LoggerProvider.Initialize(logger);

// --- 3. View + Factory ---
GameView view = new GameView();
FighterFactory factory = new FighterFactory();

// --- 4. Choix des créatures (via Factory) ---
Console.WriteLine();
view.ShowPlayerPrompt(1);
Fighter fighter1 = ChooseFighter();

Console.WriteLine();
view.ShowPlayerPrompt(2);
Fighter fighter2 = ChooseFighter();

// --- 5. Controller + Observers ---
GameRules rules = new GameRules();
GameController controller = new GameController(fighter1, fighter2, rules, view);

CombatLogger combatLogger = new CombatLogger();
ScoreTracker scoreTracker = new ScoreTracker();
BattleHistory history = new BattleHistory();

controller.AddObserver(combatLogger);
controller.AddObserver(scoreTracker);
controller.AddObserver(history);

// --- 6. Lancement ---
controller.Run();

// --- 7. Post-combat ---
scoreTracker.PrintSummary();

// =================== FONCTIONS ===================

Fighter ChooseFighter()
{
    int choice = view.AskFighterType();
    FighterType type = FighterType.All[choice - 1];
    string name = view.AskFighterName(type.Name);
    return factory.Create(type, name);
}

// Pour changer de logger, il suffit de modifier la ligne Register<ILogger, ...> :
//   ConsoleLogger  → affichage console
//   SilentLogger   → aucun affichage
//   FileLogger     → écriture fichier (nécessite un constructeur sans paramètre ou une surcharge Register)
