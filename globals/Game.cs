namespace AngryBird.Globals;

public static class Game
{
    public enum Mode
    {
        Easy,
        Hard,
    }

    public static Mode CurrentMode { get; set; } = Mode.Easy;
    public static int LevelTotal => 6;
    public static int UnlockedLevelCount { get; set; } = 4;
}