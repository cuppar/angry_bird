using Godot;

namespace AngryBird;

public partial class Camera : Camera2D
{
    [Export] public Marker2D LeftLimit = null!;
    [Export] public Marker2D RightLimit = null!;

    public override void _Ready()
    {
        base._Ready();
        LimitLeft = (int)LeftLimit.GlobalPosition.X;
        LimitRight = (int)RightLimit.GlobalPosition.X;
        ResetSmoothing();
    }
}