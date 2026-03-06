using ArenaLite.Logging;
using ArenaLite.Models;

namespace ArenaLite.UI;

/// <summary>
/// Gère l'interaction avec l'utilisateur.
/// Les prompts interactifs utilisent Console (toujours visibles).
/// Les messages de combat passent par le logger (configurable).
/// </summary>
public class GameInterface
{
    private readonly ILogger logger;

    public GameInterface(ILogger logger)
    {
        this.logger = logger;
    }

    // ========== Messages de combat (via ILogger) ==========

    public void ShowFightStart(Fighter f1, Fighter f2)
    {
        logger.Log($"\n>> {f1.Name} ({f1.Type.Name}) VS {f2.Name} ({f2.Type.Name}) — FIGHT!\n");
    }

    public void ShowTurnHeader(string attackerName)
    {
        logger.Log($"-- Tour de {attackerName} --");
    }

    public void ShowAttack(string atkName, string defName, int damage, double mult)
    {
        string bonusText = mult > 1.0 ? " (super efficace!)" : mult < 1.0 ? " (pas très efficace...)" : "";
        logger.Log($"  {atkName} attaque {defName} pour {damage} dégâts !{bonusText}");
    }

    public void ShowSpecial(string message)
    {
        logger.Log(message);
    }

    public void ShowPoison(string name, int turnsLeft)
    {
        logger.Log($"  [Poison] {name} perd 8 HP ! ({turnsLeft} tours restants)");
    }

    public void ShowStatus(Fighter f1, Fighter f2)
    {
        logger.Log($"  [ {f1.Name}: {f1.Hp}/{f1.MaxHp} HP | {f2.Name}: {f2.Hp}/{f2.MaxHp} HP ]\n");
    }

    public void ShowWinner(Fighter winner)
    {
        logger.Log($"\n*** {winner.Name} remporte le combat ! ***");
    }

    // ========== Prompts interactifs (toujours Console) ==========

    public void ShowPlayerPrompt(int playerNumber)
    {
        Console.WriteLine($"--- Joueur {playerNumber}, choisis ta créature ---");
    }

    public int AskAction()
    {
        return ReadChoice("Action ? 1) Attack  2) Special : ", 1, 2);
    }

    public int AskFighterType()
    {
        for (int i = 0; i < FighterType.All.Length; i++)
        {
            var ft = FighterType.All[i];
            Console.WriteLine($"  {i + 1}) {ft.Name,-10} — Spécial = {ft.SpecialDescription}");
        }
        return ReadChoice($"  Ton choix (1-{FighterType.All.Length}) : ", 1, FighterType.All.Length);
    }

    public string AskFighterName(string defaultName)
    {
        Console.Write("  Nom de ta créature : ");
        string name = Console.ReadLine() ?? defaultName;
        if (string.IsNullOrWhiteSpace(name)) name = defaultName;
        return name;
    }

    private int ReadChoice(string prompt, int min, int max)
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
}
