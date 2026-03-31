namespace Arena.DataAccess.Entities;

public class BattleEntity
{
    public Guid Id { get; set; }
    public Guid Creature1Id { get; set; }
    public Guid Creature2Id { get; set; }
    public Guid? WinnerId { get; set; }
    public DateTime PlayedAt { get; set; }

    public CreatureEntity Creature1 { get; set; } = null!;
    public CreatureEntity Creature2 { get; set; } = null!;
    public CreatureEntity? Winner { get; set; }

    public List<BattleLogEntity> Logs { get; set; } = new();
}
