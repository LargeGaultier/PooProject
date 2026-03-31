namespace Arena.Domain.Entities;

public class Battle
{
    public Guid Id { get; private set; }
    public Guid Creature1Id { get; private set; }
    public Guid Creature2Id { get; private set; }
    public Guid? WinnerId { get; private set; }
    public DateTime PlayedAt { get; private set; }

    private readonly List<BattleLog> _logs;
    public IReadOnlyList<BattleLog> Logs => _logs.AsReadOnly();

    /// <summary>Reconstruction depuis la persistance.</summary>
    public Battle(Guid id, Guid creature1Id, Guid creature2Id, Guid? winnerId, DateTime playedAt, List<BattleLog> logs)
    {
        Id = id;
        Creature1Id = creature1Id;
        Creature2Id = creature2Id;
        WinnerId = winnerId;
        PlayedAt = playedAt;
        _logs = logs;
    }

    /// <summary>Exécute un combat tour par tour entre deux créatures.</summary>
    public static Battle Run(Creature creature1, Creature creature2)
    {
        var logs = new List<BattleLog>();

        logs.Add(new BattleLog(Guid.NewGuid(), 0,
            $"Combat entre {creature1.Name} et {creature2.Name} !"));

        int turn = 1;
        while (creature1.IsAlive && creature2.IsAlive)
        {
            // Créature 1 attaque Créature 2
            var damage1 = creature2.TakeDamage(creature1.Attack);
            logs.Add(new BattleLog(Guid.NewGuid(), turn,
                $"Tour {turn} : {creature1.Name} attaque {creature2.Name} pour {damage1} dégâts. (HP {creature2.Name}: {creature2.CurrentHp})"));

            if (!creature2.IsAlive) break;

            // Créature 2 attaque Créature 1
            var damage2 = creature1.TakeDamage(creature2.Attack);
            logs.Add(new BattleLog(Guid.NewGuid(), turn,
                $"Tour {turn} : {creature2.Name} attaque {creature1.Name} pour {damage2} dégâts. (HP {creature1.Name}: {creature1.CurrentHp})"));

            turn++;
        }

        var winner = creature1.IsAlive ? creature1 : creature2;
        logs.Add(new BattleLog(Guid.NewGuid(), turn,
            $"{winner.Name} remporte le combat !"));

        return new Battle(Guid.NewGuid(), creature1.Id, creature2.Id, winner.Id, DateTime.UtcNow, logs);
    }
}
