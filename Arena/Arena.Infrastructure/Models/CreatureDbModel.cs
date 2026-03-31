namespace Arena.Infrastructure.Models;

public class CreatureDbModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int MaxHp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public string SpecialPower { get; set; } = string.Empty;
}
