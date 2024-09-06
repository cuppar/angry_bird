using System;
using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Bomb : Node2D
{
    private const int FragmentCount = 20;

    public override void _Ready()
    {
        base._Ready();
        RigidBody.ContactMonitor = true;
        RigidBody.MaxContactsReported = 8;
        RigidBody.BodyEntered += OnBodyEntered;
        ResourceLoader.LoadThreadedRequest(PrefabPaths.Character.BombFragment);
    }

    private PackedScene? _bombFragmentPrefab;

    private void OnBodyEntered(Node body)
    {
        if (body is not Bird) return;
        Boom();
    }

    public void Boom()
    {
        var breakPos = RigidBody.GlobalPosition;
        RigidBody.QueueFree();
        AnimationPlayer.Play("boom");
        Game.ShakeCamera(20);

        _bombFragmentPrefab ??= (PackedScene)ResourceLoader.LoadThreadedGet(PrefabPaths.Character.BombFragment);
        for (var i = 0; i < FragmentCount; i++)
        {
            var bombFragment = _bombFragmentPrefab.Instantiate<BombFragment>();
            const float minAngle = (float)Math.PI * 2 / FragmentCount;
            bombFragment.Direction = Vector2.Right.Rotated(i * minAngle);
            CallDeferred(Node.MethodName.AddChild, bombFragment);
            var pos = breakPos + bombFragment.Direction *
                ((CircleShape2D)bombFragment.GetNode<CollisionShape2D>("CollisionShape2D").Shape).Radius * 3;
            bombFragment.SetDeferred(Node2D.PropertyName.GlobalPosition, pos);
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public RigidBody2D RigidBody { get; set; } = null!;

    [Export] public AnimationPlayer AnimationPlayer { get; set; } = null!;

    #endregion
}