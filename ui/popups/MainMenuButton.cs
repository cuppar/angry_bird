using AngryBird.Autoloads;
using AngryBird.Classes;
using AngryBird.Constants;
using Godot;

namespace AngryBird.UI;

public partial class MainMenuButton : ImageButton
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
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.TitleScreen);
    }
}