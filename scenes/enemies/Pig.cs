using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Pig : Node2D
{
    [Export] public float DeathForce { get; set; } = 100;
    [Export] public int Score { get; set; } = 200;

    public override void _Ready()
    {
        base._Ready();
        RigidBody.AddToGroup(Groups.Pigs);
        HurtBox.BodyEntered += OnBodyEntered;
        ScoreLabel.Text = Score.ToString();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (GlobalPosition.X < Game.CurrentLevel.LeftLimit.GlobalPosition.X
            || GlobalPosition.X > Game.CurrentLevel.RightLimit.GlobalPosition.X)
            Die();
    }

    private void Die()
    {
        SetPhysicsProcess(false);
        RigidBody.QueueFree();
        Game.CurrentLevel.Score += Score;
        AnimationPlayer.Play("die");
        DieSFX.Play();
        CleanUp();
    }

    private async void CleanUp()
    {
        await ToSignal(AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        await ToSignal(DieSFX, AudioStreamPlayer2D.SignalName.Finished);
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not RigidBody2D rigidBody) return;
        var relativeVelocity = rigidBody.LinearVelocity - RigidBody.LinearVelocity;
        var force = relativeVelocity * rigidBody.Mass;
        if (force.Length() > DeathForce)
            Die();
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Area2D HurtBox { get; set; } = null!;

    [Export] public AnimationPlayer AnimationPlayer { get; set; } = null!;
    [Export] public CollisionShape2D CollisionShape { get; set; } = null!;
    [Export] public Label ScoreLabel { get; set; } = null!;
    [Export] public RigidBody2D RigidBody { get; set; } = null!;
    [Export] public AudioStreamPlayer2D DieSFX { get; set; } = null!;

    #endregion
}