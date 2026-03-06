namespace ArenaLite.Observers;

/// <summary>
/// Observer — Enregistre la séquence complète des événements du combat.
/// </summary>
public class BattleHistory : ICombatObserver
{
    private readonly List<string> events = new();

    public void OnCombatEvent(CombatEvent evt)
    {
        events.Add(evt.Message);
    }

    /// <summary>
    /// Historique complet, exploitable pour export, replay, etc.
    /// </summary>
    public IReadOnlyList<string> Events => events;
}
