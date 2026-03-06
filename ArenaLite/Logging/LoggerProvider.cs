namespace ArenaLite.Logging;

/// <summary>
/// Singleton — Garantit une instance unique du logger dans toute l'application.
/// Initialisé une seule fois au démarrage, utilisé partout via Instance.
/// </summary>
public class LoggerProvider
{
    private static LoggerProvider? instance;

    public ILogger Logger { get; }

    private LoggerProvider(ILogger logger)
    {
        Logger = logger;
    }

    public static LoggerProvider Instance => instance
        ?? throw new InvalidOperationException("LoggerProvider non initialisé. Appelez Initialize() d'abord.");

    public static void Initialize(ILogger logger)
    {
        if (instance != null)
            throw new InvalidOperationException("LoggerProvider déjà initialisé.");
        instance = new LoggerProvider(logger);
    }
}
