namespace ArenaLite.Logging;

/// <summary>
/// Écrit les messages dans la console.
/// </summary>
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}
