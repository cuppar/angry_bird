using Godot;

namespace AngryBird;

public partial class GlassFragment : RigidBody2D
{
    private bool _isFirstTick = true;
    [Export] public float MaxForce { get; set; } = 3000;
    [Export] public Vector2 Direction { get; set; } = Vector2.Right;

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (_isFirstTick)
        {
            _isFirstTick = false;
            var force = Direction.Normalized() * MaxForce;
            state.ApplyForce(force);
        }
    }
}