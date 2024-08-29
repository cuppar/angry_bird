using AngryBird.Autoload;
using AngryBird.Constants;
using Godot;

namespace AngryBird.UI;

public partial class TitleScreen : Control
{
    public override void _Ready()
    {
        base._Ready();
        AutoloadManager.SoundManager.SetupUISounds(this);
        AutoloadManager.SoundManager.PlayBGM(ResourceLoader.Load<AudioStream>(BGMPaths.Master));

        StartButton.Pressed += OnStartButtonPressed;
        QuitButton.Pressed += OnQuitButtonPressed;
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnStartButtonPressed()
    {
        AutoloadManager.SceneTranslation.ChangeSceneToFileAsync(ScenePaths.TestWorld);
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Button StartButton { get; set; } = null!;

    [Export] public Button QuitButton { get; set; } = null!;

    #endregion
}