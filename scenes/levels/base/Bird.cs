using Godot;

namespace AngryBird;

public partial class Bird : RigidBody2D
{
    [Export] public Vector2 InitForce = new(1000, -1000);
    private bool _isFly;

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (!_isFly)
        {
            state.ApplyForce(InitForce);
            Rotation = InitForce.Angle();
            _isFly = true;
        }
    }

    public bool IsOnScreen()
    {
        var spriteRect = Sprite.GetRect();
        var spriteGlobalRect = spriteRect with { Position = spriteRect.Position + GlobalPosition };
        var outOfLeft = spriteGlobalRect.Position.X + spriteGlobalRect.Size.X < LeftLimit.GlobalPosition.X;
        var outOfRight = spriteGlobalRect.Position.X > RightLimit.GlobalPosition.X;

        return !(outOfLeft || outOfRight);
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    public Marker2D LeftLimit = null!;

    [Export] public Marker2D RightLimit = null!;
    [Export] public Sprite2D Sprite = null!;

    #endregion
}