namespace Arena.Infrastructure.Models;

public class BattleLogDbModel
{
    public Guid Id { get; set; }
    public Guid BattleId { get; set; }
    public int Turn { get; set; }
    public string Description { get; set; } = string.Empty;

    public BattleDbModel Battle { get; set; } = null!;
}
