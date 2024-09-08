using System;
using AngryBird.Constants;
using Godot;

namespace AngryBird;

public partial class Glass : Node2D
{
    private PackedScene? _glassFragmentPackedScene;
    [Export] public float DeathForce { get; set; } = 200;

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public RigidBody2D RigidBody { get; set; } = null!;

    #endregion

    public override void _Ready()
    {
        base._Ready();
        RigidBody.ContactMonitor = true;
        RigidBody.MaxContactsReported = 8;
        RigidBody.BodyEntered += OnBodyEntered;
        ResourceLoader.LoadThreadedRequest(PrefabPaths.Character.GlassFragment);
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not RigidBody2D hitter) return;
        var relativeVelocity = hitter.LinearVelocity - RigidBody.LinearVelocity;
        var force = relativeVelocity * hitter.Mass;
        if (force.Length() > DeathForce)
            Break();
    }

    private const int FragmentCount = 8;

    private void Break()
    {
        var breakPos = RigidBody.GlobalPosition;
        RigidBody.QueueFree();
        _glassFragmentPackedScene ??= (PackedScene)ResourceLoader.LoadThreadedGet(PrefabPaths.Character.GlassFragment);
        BreakSFX.Play();
        
        for (var i = 0; i < FragmentCount; i++)
        {
            var glassFragment = _glassFragmentPackedScene.Instantiate<GlassFragment>();
            const float minAngle = (float)Math.PI * 2 / FragmentCount;
            glassFragment.Direction = Vector2.Right.Rotated(i * minAngle);
            CallDeferred(Node.MethodName.AddChild, glassFragment);
            var pos = breakPos + glassFragment.Direction *
                ((RectangleShape2D)glassFragment.GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.X * 3;
            glassFragment.SetDeferred(Node2D.PropertyName.GlobalPosition, pos);
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public AudioStreamPlayer2D BreakSFX { get; set; } = null!;

    #endregion
}