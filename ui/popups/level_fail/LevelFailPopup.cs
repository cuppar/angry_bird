using Godot;

namespace AngryBird.UI;

public partial class LevelFailPopup : PopupPanel
{
    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public LevelFailPanel LevelFailPanel { get; set; } = null!;

    #endregion

    public override void _Ready()
    {
        base._Ready();
        Hide();
        PopupWindow = false;
        LevelFailPanel.Close += () =>
        {
            GetTree().Paused = false;
            Hide();
        };
    }
}