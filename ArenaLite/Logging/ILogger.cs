namespace ArenaLite.Logging;

/// <summary>
/// Capacité "écrire un message de log".
/// Les classes métier dépendent de cette abstraction,
/// jamais d'un détail (console, fichier…).
/// </summary>
public interface ILogger
{
    void Log(string message);
}
