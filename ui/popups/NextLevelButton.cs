using AngryBird.Autoloads;
using AngryBird.Classes;
using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird.UI;

public partial class NextLevelButton : ImageButton
{
    #region Delegates

    [Signal]
    public delegate void CloseEventHandler();

    #endregion

    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        EmitSignal(SignalName.Close);
        if (Game.CurrentLevel == Game.LevelTotal)
        {
            AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.GamePassScreen);
        }
        else
        {
            Game.CurrentLevel += 1;
            AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.GetLevel(Game.CurrentLevel));
        }
    }
}