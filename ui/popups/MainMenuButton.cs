using AngryBird.Autoloads;
using AngryBird.Classes;
using AngryBird.Constants;

namespace AngryBird.UI;

public partial class MainMenuButton : ImageButton
{
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.TitleScreen);
    }
}