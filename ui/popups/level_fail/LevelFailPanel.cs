using AngryBird.Autoloads;
using Godot;

namespace AngryBird.UI;

public partial class LevelFailPanel : Control
{
    #region Delegates

    [Signal]
    public delegate void CloseEventHandler();

    #endregion

    public override void _Ready()
    {
        base._Ready();
        AutoloadManager.SoundManager.SetupUISounds(this);
        MainMenuButton.Close += OnClose;
        ReplayButton.Close += OnClose;
    }

    private void OnClose()
    {
        EmitSignal(SignalName.Close);
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public MainMenuButton MainMenuButton { get; set; } = null!;

    [Export] public ReplayButton ReplayButton { get; set; } = null!;

    #endregion
}