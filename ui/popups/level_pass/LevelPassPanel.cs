using AngryBird.Autoloads;
using AngryBird.Constants;
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
        if (Visible && IsVisibleInTree())
        {
            AutoloadManager.SoundManager.PlaySFX(SFXNames.LevelPass);
            ScoreLabel.Text = $"得分：{Game.CurrentLevel.Score}, 剩余 {Game.CurrentLevel.LiveLeft} 次机会";
            FinalScoreLabel.Text = $"最终得分：{Game.CurrentLevel.Score * (Game.CurrentLevel.LiveLeft + 1)}";
        }
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