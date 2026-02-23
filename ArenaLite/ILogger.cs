// Version pédagogique volontairement mal conçue (anti-patterns)
interface ILogger
{
    void Log(string message);
    void LogWarning(string message);
    void LogError(string message);
}

class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("[LOG] " + message);
        Console.ResetColor();
    }

    public void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[WARN] " + message);
        Console.ResetColor();
    }

    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERR] " + message);
        Console.ResetColor();
    }
}
