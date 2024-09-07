using Godot;

namespace AngryBird.UI;

public partial class LevelPassPopup : PopupPanel
{
    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public LevelPassPanel LevelPassPanel { get; set; } = null!;

    #endregion

    public override void _Ready()
    {
        base._Ready();
        Hide();
        PopupWindow = false;
        LevelPassPanel.Close += () =>
        {
            GetTree().Paused = false;
            Hide();
        };
    }
}