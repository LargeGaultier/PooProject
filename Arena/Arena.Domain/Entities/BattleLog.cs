namespace Arena.Domain.Entities;

public class BattleLog
{
    public Guid Id { get; }
    public int Turn { get; }
    public string Description { get; }

    public BattleLog(Guid id, int turn, string description)
    {
        Id = id;
        Turn = turn;
        Description = description;
    }
}
