using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Camera : Camera2D
{
    [Export] public Marker2D LeftLimit = null!;
    [Export] public Marker2D RightLimit = null!;

    [Export] public float Strength { get; set; }
    [Export] public float RecoverySpeed { get; set; } = 16;

    public override void _Ready()
    {
        base._Ready();
        LimitLeft = (int)LeftLimit.GlobalPosition.X;
        LimitRight = (int)RightLimit.GlobalPosition.X;
        ResetSmoothing();
        Game.ShakeCameraEvent += amount => Strength += amount;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Offset = new Vector2((float)GD.RandRange(-Strength, Strength), (float)GD.RandRange(-Strength, Strength));
        Strength = Mathf.MoveToward(Strength, 0, RecoverySpeed * (float)delta);
    }
}