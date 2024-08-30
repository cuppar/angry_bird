using AngryBird.Globals;
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

        EasyButton.Pressed += OnEasyButtonPressed;
        HardButton.Pressed += OnHardButtonPressed;
        QuitButton.Pressed += OnQuitButtonPressed;
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnEasyButtonPressed()
    {
        Game.CurrentMode = Game.Mode.Easy;
        AutoloadManager.SceneTranslation.ChangeSceneToFileAsync(ScenePaths.TestWorld);
    }

    private void OnHardButtonPressed()
    {
        Game.CurrentMode = Game.Mode.Hard;
        AutoloadManager.SceneTranslation.ChangeSceneToFileAsync(ScenePaths.TestWorld);
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Button EasyButton { get; set; } = null!;

    [Export] public Button HardButton { get; set; } = null!;

    [Export] public Button QuitButton { get; set; } = null!;

    #endregion
}