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
            _isFly = true;
        }
    }
}