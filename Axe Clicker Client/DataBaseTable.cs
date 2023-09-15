using SQLite.Attributes;

public class SettingsTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public bool isFullscreen { get; set; }
    public int qualityLevel { get; set; }
    public int framerate { get; set; }
    public float volume { get; set; }
    public int screenWidth { get; set; }
    public int screenHeight { get; set; }
    public int screenRefreshRate { get; set; }
}
