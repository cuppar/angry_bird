using System;
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

    private const string SaveFilePath = "user://save.sav";

    public static Mode CurrentMode { get; set; } = Mode.Easy;
    public static int CurrentLevelNumber { get; set; } = -1;
    public static int LevelTotal => 5;
    public static Level CurrentLevel { get; set; } = null!;
    public static event Action<float>? ShakeCameraEvent;

    public static void ShakeCamera(float amount)
    {
        ShakeCameraEvent?.Invoke(amount);
    }

    private static void Save()
    {
        using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
        file.Store32((uint)UnlockedLevelCount);
    }

    public static void Load()
    {
        using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
        UnlockedLevelCount = (int)file.Get32();
    }

    public static bool HasLoadFile()
    {
        return FileAccess.FileExists(SaveFilePath);
    }

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
        Save();
    }

    #endregion
}