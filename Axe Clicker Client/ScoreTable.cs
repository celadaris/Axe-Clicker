using SQLite.Attributes;

public class ScoreTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int highScore { get; set; } = 0;
    public string playerName { get; set; } = string.Empty;
}
