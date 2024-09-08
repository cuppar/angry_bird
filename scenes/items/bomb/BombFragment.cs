using Godot;

namespace AngryBird;

public partial class BombFragment : RigidBody2D
{
    private bool _isFly;
    [Export] public float Force = 100000;
    [Export] public Vector2 Direction { get; set; } = Vector2.Zero;

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (!_isFly)
        {
            _isFly = true;
            state.ApplyForce(Force * Direction.Normalized());
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (IsSleeping())
            QueueFree();
    }
}