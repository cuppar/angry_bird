using AngryBird.Autoloads;
using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird.UI;

public partial class LevelSelectScreen : Control
{
    private PackedScene _levelButtonPrefab = null!;

    public override void _Ready()
    {
        base._Ready();
        ReturnButton.Pressed += OnReturnButtonPressed;
        _levelButtonPrefab = ResourceLoader.Load<PackedScene>(PrefabPaths.UI.LevelButton);
        SetupLevelButtons();
        AutoloadManager.SoundManager.SetupUISounds(this);
    }

    private void OnReturnButtonPressed()
    {
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.TitleScreen);
    }

    private void SetupLevelButtons()
    {
        var buttonCount = Game.LevelTotal;
        var unlockedCount = Game.UnlockedLevelCount;

        for (var i = 0; i < buttonCount; i++)
        {
            var levelButton = _levelButtonPrefab.Instantiate<LevelButton>();
            levelButton.Level = i + 1;
            levelButton.Locked = i >= unlockedCount;
            LevelButtonContainer.AddChild(levelButton);
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Button ReturnButton { get; set; } = null!;

    [Export] public GridContainer LevelButtonContainer { get; set; } = null!;

    #endregion
}