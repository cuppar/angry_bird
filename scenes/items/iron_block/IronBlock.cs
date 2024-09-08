using Godot;

namespace AngryBird;

public partial class IronBlock : RigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
        MaxContactsReported = 8;
        ContactMonitor = true;
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        // todo 铁块音效
        GD.Print($"铁块音效");
    }
}