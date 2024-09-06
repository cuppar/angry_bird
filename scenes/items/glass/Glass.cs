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

    private void Break()
    {
        var breakPos = RigidBody.GlobalPosition;
        RigidBody.QueueFree();
        _glassFragmentPackedScene ??= (PackedScene)ResourceLoader.LoadThreadedGet(PrefabPaths.Character.GlassFragment);
        for (var i = 0; i < 4; i++)
        {
            var glassFragment = _glassFragmentPackedScene.Instantiate<GlassFragment>();
            CallDeferred(Node.MethodName.AddChild, glassFragment);
            glassFragment.SetDeferred(Node2D.PropertyName.GlobalPosition, breakPos);
        }
    }
}