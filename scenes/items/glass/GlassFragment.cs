using Godot;

namespace AngryBird;

public partial class GlassFragment : RigidBody2D
{
    [Export] public float MaxForce { get; set; } = 5000;

    private bool _isFirstTick = true;

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (_isFirstTick)
        {
            _isFirstTick = false;
            var force = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)).Normalized() * MaxForce;
            state.ApplyForce(force);
        }
    }
}