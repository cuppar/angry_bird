using AngryBird.Autoloads;
using AngryBird.Classes;
using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird.UI;

public partial class NextLevelButton : ImageButton
{
    [Signal]
    public delegate void CloseEventHandler();
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        EmitSignal(SignalName.Close);
        if (Game.CurrentLevel == Game.LevelTotal)
            // todo: 改为跳转到总的游戏通关界面
        {
            AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.TitleScreen);
        }
        else
        {
            Game.CurrentLevel += 1;
            AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.GetLevel(Game.CurrentLevel));
        }
    }
}