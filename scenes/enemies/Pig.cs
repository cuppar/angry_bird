using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Pig : RigidBody2D
{
    [Export] public float DeathForce { get; set; } = 100;
    [Export] public int Score { get; set; } = 200;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Groups.Pigs);
        HurtBox.BodyEntered += OnBodyEntered;
        ScoreLabel.Text = Score.ToString();
    }

    private void Die()
    {
        Game.Score += Score;
        AnimationPlayer.Play("die");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not RigidBody2D rigidBody) return;
        var relativeVelocity = rigidBody.LinearVelocity - LinearVelocity;
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

    #endregion
}