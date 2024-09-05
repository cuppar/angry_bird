using AngryBird.Autoloads;
using AngryBird.Constants;
using AngryBird.Globals;
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

        if (Game.HasLoadFile())
            Game.Load();
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnEasyButtonPressed()
    {
        Game.CurrentMode = Game.Mode.Easy;
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.LevelSelectionScreen);
    }

    private void OnHardButtonPressed()
    {
        Game.CurrentMode = Game.Mode.Hard;
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.LevelSelectionScreen);
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Button EasyButton { get; set; } = null!;

    [Export] public Button HardButton { get; set; } = null!;

    [Export] public Button QuitButton { get; set; } = null!;

    #endregion
}