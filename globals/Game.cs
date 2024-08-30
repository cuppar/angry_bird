namespace angry_bird.Globals;

public static class Game
{
    public enum Mode
    {
        Easy,
        Hard,
    }

    public static Mode CurrentMode { get; set; } = Mode.Easy;
}