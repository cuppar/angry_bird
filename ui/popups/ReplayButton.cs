using AngryBird.Classes;
using Godot;

namespace AngryBird.UI;

public partial class ReplayButton : ImageButton
{
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene);
    }
}