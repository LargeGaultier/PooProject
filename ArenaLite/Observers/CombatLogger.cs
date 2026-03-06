using ArenaLite.Logging;

namespace ArenaLite.Observers;

/// <summary>
/// Observer — Affiche les événements de combat via le logger (Singleton).
/// </summary>
public class CombatLogger : ICombatObserver
{
    public void OnCombatEvent(CombatEvent evt)
    {
        LoggerProvider.Instance.Logger.Log(evt.Message);
    }
}
