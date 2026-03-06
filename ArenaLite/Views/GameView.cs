using ArenaLite.Models;

namespace ArenaLite.Views;

/// <summary>
/// MVC View — Gère uniquement l'interaction utilisateur (prompts + saisie).
/// Aucune logique métier. Aucun affichage de combat (c'est le rôle des observers).
/// </summary>
public class GameView
{
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
