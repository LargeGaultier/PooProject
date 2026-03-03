using ArenaLite.Models;
using ArenaLite.UI;

namespace ArenaLite.Core;

/// <summary>
/// Orchestre le déroulement du combat tour par tour.
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
                int damage = rules.DoAttack(attacker, defender);
                double mult = rules.GetTypeBonus(attacker.Type, defender.Type);
                ui.ShowAttack(attacker.Name, defender.Name, damage, mult);
            }
            else
            {
                ExecuteSpecial(attacker, defender);
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

    private void ExecuteSpecial(Fighter attacker, Fighter defender)
    {
        if (attacker.Type == FighterType.Fire)
        {
            int damage = rules.DoSpecialFire(attacker, defender);
            ui.ShowFireSpecial(attacker.Name, defender.Name, damage);
        }
        else if (attacker.Type == FighterType.Water)
        {
            int heal = rules.DoSpecialWater(attacker);
            ui.ShowWaterSpecial(attacker.Name, heal);
        }
        else // Grass
        {
            rules.DoSpecialGrass(defender);
            ui.ShowGrassSpecial(attacker.Name, defender.Name);
        }
    }
}
