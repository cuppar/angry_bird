using AngryBird.Autoloads;
using AngryBird.Classes;
using AngryBird.Constants;
using AngryBird.Globals;

namespace AngryBird.UI;

public partial class NextLevelButton : ImageButton
{
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
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