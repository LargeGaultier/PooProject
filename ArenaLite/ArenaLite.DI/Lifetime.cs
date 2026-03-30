namespace ArenaLite.DI;

/// <summary>
/// Cycle de vie d'une dépendance dans le container.
/// </summary>
public enum Lifetime
{
    /// <summary>
    /// Une nouvelle instance est créée à chaque résolution.
    /// </summary>
    Transient,

    /// <summary>
    /// Une seule instance est créée puis réutilisée.
    /// </summary>
    Singleton
}
