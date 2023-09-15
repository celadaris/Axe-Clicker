using SQLite.Attributes;

public class LoginTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public bool autoLogin { get; set; } = false;
    public string playerName { get; set; } = string.Empty;
    public string pwrdHash { get; set; } = string.Empty;
}
