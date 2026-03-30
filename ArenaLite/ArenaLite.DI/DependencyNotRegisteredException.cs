namespace ArenaLite.DI;

public class DependencyNotRegisteredException : Exception
{
    public DependencyNotRegisteredException(Type type)
        : base($"Le type '{type.FullName}' n'est pas enregistré dans le container.")
    {
    }
}
