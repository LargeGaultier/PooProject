using ArenaLite.Logging;

namespace ArenaLite.Observers;

/// <summary>
/// Observer — Suit les statistiques du combat (attaques, spéciaux, poison, tours).
/// </summary>
public class ScoreTracker : ICombatObserver
{
    private int attacks;
    private int specials;
    private int poisonTicks;
    private int totalAttackDamage;
    private int turns;

    public void OnCombatEvent(CombatEvent evt)
    {
        switch (evt.Type)
        {
            case "turn":    turns++; break;
            case "attack":  attacks++; totalAttackDamage += evt.Value; break;
            case "special": specials++; break;
            case "poison":  poisonTicks++; break;
        }
    }

    public void PrintSummary()
    {
        var logger = LoggerProvider.Instance.Logger;
        logger.Log("\n--- Statistiques du combat ---");
        logger.Log($"  Tours joués       : {turns}");
        logger.Log($"  Attaques          : {attacks} ({totalAttackDamage} dégâts)");
        logger.Log($"  Spéciaux utilisés : {specials}");
        logger.Log($"  Ticks de poison   : {poisonTicks}");
    }
}
