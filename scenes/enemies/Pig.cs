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

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (GlobalPosition.X < Game.CurrentLevel.LeftLimit.GlobalPosition.X
            || GlobalPosition.X > Game.CurrentLevel.RightLimit.GlobalPosition.X)
            Die();
    }

    private void Die()
    {
        Game.CurrentLevel.Score += Score;
        AnimationPlayer.Play("die");
        // todo 猪死音效
        GD.Print($"猪死亡音效");
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