using Godot;
using AngryBird.Classes;

namespace AngryBird;

public partial class Player : CharacterBody2D, IStateMachine<Player.State>
{
    public enum State
    {
        Idle,
    }

    private StateMachine<State> _stateMachine;

    private Player()
    {
        _stateMachine = StateMachine<State>.Create(this);
    }

    public void TransitionState(State fromState, State toState)
    {
    }

    public State GetNextState(State currentState, out bool keepCurrent)
    {
        keepCurrent = true;
        return currentState;
    }

    public void TickPhysics(State currentState, double delta)
    {
    }
}