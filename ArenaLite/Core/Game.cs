using ArenaLite.Models;
using ArenaLite.UI;

namespace ArenaLite.Core;

/// <summary>
/// Orchestre le déroulement du combat tour par tour.
/// Ne connaît pas les types concrets de créatures (polymorphisme).
/// </summary>
public class Game
{
    private readonly Fighter fighter1;
    private readonly Fighter fighter2;
    private readonly GameRules rules;
    private readonly GameInterface ui;

    public Game(Fighter fighter1, Fighter fighter2, GameRules rules, GameInterface ui)
    {
        this.fighter1 = fighter1;
        this.fighter2 = fighter2;
        this.rules = rules;
        this.ui = ui;
    }

    public void Run()
    {
        ui.ShowFightStart(fighter1, fighter2);

        int turn = 0;
        while (fighter1.IsAlive && fighter2.IsAlive)
        {
            bool isPlayer1 = (turn % 2 == 0);
            Fighter attacker = isPlayer1 ? fighter1 : fighter2;
            Fighter defender = isPlayer1 ? fighter2 : fighter1;

            ui.ShowTurnHeader(attacker.Name);
            int choice = ui.AskAction();

            if (choice == 1)
            {
                var (damage, mult) = rules.DoAttack(attacker, defender);
                ui.ShowAttack(attacker.Name, defender.Name, damage, mult);
            }
            else
            {
                // Appel polymorphique : chaque Fighter sait exécuter son propre spécial
                string message = attacker.UseSpecial(defender);
                ui.ShowSpecial(message);
            }

            // Poison en fin de tour
            if (rules.ApplyPoison(defender))
            {
                ui.ShowPoison(defender.Name, defender.PoisonTurns);
            }

            ui.ShowStatus(fighter1, fighter2);
            turn++;
        }

        Fighter winner = fighter1.IsAlive ? fighter1 : fighter2;
        ui.ShowWinner(winner);
    }
}
