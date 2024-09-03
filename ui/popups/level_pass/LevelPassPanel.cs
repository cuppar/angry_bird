using AngryBird.Autoloads;
using Godot;

namespace AngryBird.UI;

public partial class LevelPassPanel : Control
{
    #region Delegates

    [Signal]
    public delegate void CloseEventHandler();

    #endregion

    public override void _Ready()
    {
        base._Ready();
        AutoloadManager.SoundManager.SetupUISounds(this);
        MainMenuButton.Pressed += OnButtonPressed;
        ReplayButton.Pressed += OnButtonPressed;
        NextLevelButton.Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        EmitSignal(SignalName.Close);
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public MainMenuButton MainMenuButton { get; set; } = null!;

    [Export] public ReplayButton ReplayButton { get; set; } = null!;
    [Export] public NextLevelButton NextLevelButton { get; set; } = null!;

    #endregion
}