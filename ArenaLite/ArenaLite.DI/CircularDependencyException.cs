namespace ArenaLite.DI;

public class CircularDependencyException : Exception
{
    public CircularDependencyException(IEnumerable<Type> chain)
        : base($"Dépendance circulaire détectée : {string.Join(" → ", chain.Select(t => t.Name))}")
    {
    }
}
