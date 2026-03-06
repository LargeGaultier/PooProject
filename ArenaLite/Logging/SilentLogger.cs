namespace ArenaLite.Logging;

/// <summary>
/// Ignore tous les messages (mode silencieux).
/// </summary>
public class SilentLogger : ILogger
{
    public void Log(string message)
    {
        // Ne rien faire
    }
}
