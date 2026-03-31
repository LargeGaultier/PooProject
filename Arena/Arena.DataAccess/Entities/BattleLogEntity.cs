using System.Text.Json.Serialization;

namespace Arena.DataAccess.Entities;

public class BattleLogEntity
{
    public Guid Id { get; set; }
    public Guid BattleId { get; set; }
    public int Turn { get; set; }
    public string Description { get; set; } = string.Empty;

    [JsonIgnore]
    public BattleEntity Battle { get; set; } = null!;
}
