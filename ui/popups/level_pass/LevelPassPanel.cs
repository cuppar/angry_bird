using AngryBird.Autoloads;
using AngryBird.Globals;
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
        MainMenuButton.Close += OnClose;
        ReplayButton.Close += OnClose;
        NextLevelButton.Close += OnClose;
        VisibilityChanged += OnVisibilityChanged;
    }

    private void OnVisibilityChanged()
    {
        ScoreLabel.Text = $"得分：{Game.Score}, 剩余 {Game.LiveLeft} 次机会";
        FinalScoreLabel.Text = $"最终得分：{Game.Score * (Game.LiveLeft + 1)}";
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
    [Export] public NextLevelButton NextLevelButton { get; set; } = null!;
    [Export] public Label ScoreLabel { get; set; } = null!;
    [Export] public Label FinalScoreLabel { get; set; } = null!;

    #endregion
}