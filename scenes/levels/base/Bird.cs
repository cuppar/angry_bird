using Godot;

namespace AngryBird;

public partial class Bird : RigidBody2D
{
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (Input.IsActionPressed("ui_accept"))
            state.ApplyForce(new Vector2(1000, -1000));
    }
}