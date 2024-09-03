using AngryBird.Constants;
using Godot;

namespace AngryBird;

public partial class Pig : RigidBody2D
{
    [Export] public float DeathForce { get; set; } = 100;


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Area2D HurtBox { get; set; } = null!;

    #endregion

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Groups.Pigs);
        HurtBox.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not RigidBody2D rigidBody) return;
        var relativeVelocity = rigidBody.LinearVelocity - LinearVelocity;
        var force = relativeVelocity * rigidBody.Mass;
        if (force.Length() > DeathForce)
            QueueFree();
    }
}