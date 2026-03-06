using ArenaLite.Models;
using ArenaLite.Views;
using ArenaLite.Observers;

namespace ArenaLite.Controllers;

/// <summary>
/// MVC Controller — Orchestre le combat tour par tour.
/// Ne connaît pas les types concrets de créatures ni les observateurs.
/// Émet des CombatEvent que les observers traitent librement.
/// </summary>
public class GameController
{
    private readonly Fighter fighter1;
    private readonly Fighter fighter2;
    private readonly GameRules rules;
    private readonly GameView view;
    private readonly List<ICombatObserver> observers = new();

    public GameController(Fighter fighter1, Fighter fighter2, GameRules rules, GameView view)
    {
        this.fighter1 = fighter1;
        this.fighter2 = fighter2;
        this.rules = rules;
        this.view = view;
    }

    public void AddObserver(ICombatObserver observer)
    {
        observers.Add(observer);
    }

    public void Run()
    {
        Notify(new CombatEvent("fight_start",
            $"\n>> {fighter1.Name} ({fighter1.Type.Name}) VS {fighter2.Name} ({fighter2.Type.Name}) — FIGHT!\n"));

        int turn = 0;
        while (fighter1.IsAlive && fighter2.IsAlive)
        {
            bool isPlayer1 = (turn % 2 == 0);
            Fighter attacker = isPlayer1 ? fighter1 : fighter2;
            Fighter defender = isPlayer1 ? fighter2 : fighter1;

            Notify(new CombatEvent("turn", $"-- Tour de {attacker.Name} --"));
            int choice = view.AskAction();

            if (choice == 1)
            {
                var (damage, mult) = rules.DoAttack(attacker, defender);
                string bonusText = mult > 1.0 ? " (super efficace!)" : mult < 1.0 ? " (pas très efficace...)" : "";
                Notify(new CombatEvent("attack",
                    $"  {attacker.Name} attaque {defender.Name} pour {damage} dégâts !{bonusText}", damage));
            }
            else
            {
                string message = attacker.UseSpecial(defender);
                Notify(new CombatEvent("special", message));
            }

            // Poison en fin de tour
            if (rules.ApplyPoison(defender))
            {
                Notify(new CombatEvent("poison",
                    $"  [Poison] {defender.Name} perd {GameRules.PoisonDamagePerTurn} HP ! ({defender.PoisonTurns} tours restants)",
                    GameRules.PoisonDamagePerTurn));
            }

            Notify(new CombatEvent("status",
                $"  [ {fighter1.Name}: {fighter1.Hp}/{fighter1.MaxHp} HP | {fighter2.Name}: {fighter2.Hp}/{fighter2.MaxHp} HP ]\n"));

            turn++;
        }

        Fighter winner = fighter1.IsAlive ? fighter1 : fighter2;
        Notify(new CombatEvent("victory", $"\n*** {winner.Name} remporte le combat ! ***"));
    }

    private void Notify(CombatEvent evt)
    {
        foreach (var observer in observers)
            observer.OnCombatEvent(evt);
    }
}
