namespace ArenaLite.Observers;

/// <summary>
/// Événement émis pendant le combat.
/// Type : "attack", "special", "poison", "status", "turn", "fight_start", "victory"
/// Value : valeur numérique associée (dégâts, soin, etc.)
/// </summary>
public record CombatEvent(string Type, string Message, int Value = 0);

/// <summary>
/// Observer — Interface pour tout composant qui réagit aux événements de combat.
/// Le contrôleur ne connaît pas les implémentations concrètes.
/// </summary>
public interface ICombatObserver
{
    void OnCombatEvent(CombatEvent evt);
}
