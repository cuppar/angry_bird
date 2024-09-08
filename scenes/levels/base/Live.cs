using AngryBird.Globals.Extensions;
using Godot;

namespace AngryBird;

public partial class Live : CanvasLayer
{
    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Label LiveLabel { get; set; } = null!;

    #endregion

    #region LiveLeft

    private int _liveLeft;

    [Export]
    public int LiveLeft
    {
        get => _liveLeft;
        set => SetLiveLeft(value);
    }

    private async void SetLiveLeft(int value)
    {
        _liveLeft = value;
        await this.EnsureReadyAsync();
        LiveLabel.Text = value.ToString();
    }

    #endregion
}