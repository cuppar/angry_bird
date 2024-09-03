using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Live : CanvasLayer
{
    #region LiveLeft

    private int _liveLeft = 0;

    [Export]
    public int LiveLeft
    {
        get => _liveLeft;
        set => SetLiveLeft(value);
    }

    private async void SetLiveLeft(int value)
    {
        _liveLeft = value;
        await Helper.WaitNodeReady(this);
        LiveLabel.Text = value.ToString();
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Label LiveLabel { get; set; } = null!;

    #endregion
}