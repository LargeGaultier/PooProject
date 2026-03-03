using ArenaLite.Models;
using ArenaLite.Models.Fighters;
using ArenaLite.Core;
using ArenaLite.UI;

// ============================================================
// ArenaLite — Programme principal
// Initialisation et lancement du jeu
// ============================================================

GameInterface ui = new GameInterface();
GameRules rules = new GameRules();

ui.ShowWelcome();

ui.ShowPlayerPrompt(1);
Fighter fighter1 = ChooseFighter();

Console.WriteLine();
ui.ShowPlayerPrompt(2);
Fighter fighter2 = ChooseFighter();

Game game = new Game(fighter1, fighter2, rules, ui);
game.Run();

// =================== FONCTIONS ===================

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
