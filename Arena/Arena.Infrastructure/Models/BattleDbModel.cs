namespace Arena.Infrastructure.Models;

public class BattleDbModel
{
    public Guid Id { get; set; }
    public Guid Creature1Id { get; set; }
    public Guid Creature2Id { get; set; }
    public Guid? WinnerId { get; set; }
    public DateTime PlayedAt { get; set; }

    public CreatureDbModel Creature1 { get; set; } = null!;
    public CreatureDbModel Creature2 { get; set; } = null!;
    public CreatureDbModel? Winner { get; set; }

    public List<BattleLogDbModel> Logs { get; set; } = new();
}
