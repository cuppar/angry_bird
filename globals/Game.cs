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
    public static int CurrentLevel { get; set; } = -1;
    public static int LevelTotal => 1;


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