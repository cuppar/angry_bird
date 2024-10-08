namespace AngryBird.Constants;

public static class ScenePaths
{
    public const string TitleScreen = "res://ui/title_screen/TitleScreen.tscn";
    public const string LevelSelectionScreen = "res://ui/level_select_screen/Level_select_screen.tscn";
    public const string GamePassScreen = "res://ui/game_pass_screen/game_pass_screen.tscn";


    public static string GetLevel(int level)
    {
        return $"res://scenes/levels/level_{level:00}.tscn";
    }
}