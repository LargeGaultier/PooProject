using System.Reflection;

namespace ArenaLite.DI;

/// <summary>
/// Container IoC minimal avec support Singleton/Transient et auto-wiring.
/// </summary>
public class Container : IServiceProvider
{
    private readonly Dictionary<Type, Registration> registrations = new();
    private readonly Dictionary<Type, object> singletons = new();

    /// <summary>
    /// Enregistre un binding interface → implémentation avec un cycle de vie.
    /// </summary>
    public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
        where TImplementation : class, TInterface
    {
        registrations[typeof(TInterface)] = new Registration(typeof(TImplementation), lifetime);
    }

    /// <summary>
    /// Résout une instance du type demandé, en résolvant récursivement ses dépendances.
    /// </summary>
    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T), new HashSet<Type>());
    }

    /// <summary>
    /// Implémentation de IServiceProvider : retourne null si le type n'est pas enregistré
    /// (au lieu de lever une exception, conformément au contrat .NET).
    /// </summary>
    public object? GetService(Type serviceType)
    {
        if (!registrations.ContainsKey(serviceType))
            return null;

        return Resolve(serviceType, new HashSet<Type>());
    }

    private object Resolve(Type serviceType, HashSet<Type> resolving)
    {
        if (!registrations.TryGetValue(serviceType, out var registration))
            throw new DependencyNotRegisteredException(serviceType);

        // Détection de dépendance circulaire
        if (!resolving.Add(serviceType))
        {
            var chain = resolving.Append(serviceType);
            throw new CircularDependencyException(chain);
        }

        try
        {
            // Singleton : retourner l'instance existante si déjà créée
            if (registration.Lifetime == Lifetime.Singleton
                && singletons.TryGetValue(serviceType, out var existing))
            {
                return existing;
            }

            var instance = CreateInstance(registration.ImplementationType, resolving);

            // Singleton : stocker l'instance pour réutilisation
            if (registration.Lifetime == Lifetime.Singleton)
            {
                singletons[serviceType] = instance;
            }

            return instance;
        }
        finally
        {
            resolving.Remove(serviceType);
        }
    }

    private object CreateInstance(Type implementationType, HashSet<Type> resolving)
    {
        // Prendre le constructeur avec le plus de paramètres
        var constructor = implementationType.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault()
            ?? throw new InvalidOperationException(
                $"Aucun constructeur public trouvé pour '{implementationType.FullName}'.");

        // Résoudre chaque paramètre récursivement
        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            args[i] = Resolve(parameters[i].ParameterType, resolving);
        }

        return constructor.Invoke(args);
    }

    private record Registration(Type ImplementationType, Lifetime Lifetime);
}
