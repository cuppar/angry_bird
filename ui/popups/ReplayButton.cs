using AngryBird.Classes;
using Godot;

namespace AngryBird.UI;

public partial class ReplayButton : ImageButton
{
    #region Delegates

    [Signal]
    public delegate void CloseEventHandler();

    #endregion

    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        EmitSignal(SignalName.Close);
        GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene);
    }
}