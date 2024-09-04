using Godot;

namespace AngryBird.Globals;

public static class Game
{
    #region Mode enum

    public enum Mode
    {
        Easy,
        Hard
    }

    #endregion

    public static Mode CurrentMode { get; set; } = Mode.Easy;
    public static int CurrentLevelNumber { get; set; } = -1;
    public static int LevelTotal => 2;
    public static Level CurrentLevel { get; set; } = null!;


    #region UnlockedLevelCount

    private static int _unlockedLevelCount = 1;

    [Export]
    public static int UnlockedLevelCount
    {
        get => _unlockedLevelCount;
        set => SetUnlockedLevelCount(value);
    }

    private static void SetUnlockedLevelCount(int value)
    {
        if (value == _unlockedLevelCount) return;
        if (value < 0 || value > LevelTotal) return;
        _unlockedLevelCount = value;
    }

    #endregion
}