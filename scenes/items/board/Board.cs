using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Board : Node2D
{
    private const int ScoreFirstHit = 50;
    private const int ScoreSecondHit = 100;
    private int HitCount { get; set; }

    public override void _Ready()
    {
        base._Ready();
        RigidBody.ContactMonitor = true;
        RigidBody.MaxContactsReported = 8;
        RigidBody.BodyEntered += OnBodyEntered;
        OriginSprite.Show();
        HitSprite.Hide();
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not Bird) return;

        HitCount += 1;
        switch (HitCount)
        {
            case 1:
                Break01SFX.Play();
                Game.CurrentLevel.Score += ScoreFirstHit;
                OriginSprite.Hide();
                HitSprite.Show();
                break;
            case 2:
                Break02SFX.Play();
                Game.CurrentLevel.Score += ScoreSecondHit;
                Die();
                break;
        }
    }

    private async void Die()
    {
        RigidBody.QueueFree();
        await ToSignal(Break02SFX, AudioStreamPlayer2D.SignalName.Finished);
        QueueFree();
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D OriginSprite { get; set; } = null!;

    [Export] public Sprite2D HitSprite { get; set; } = null!;
    [Export] public AudioStreamPlayer2D Break01SFX { get; set; } = null!;
    [Export] public AudioStreamPlayer2D Break02SFX { get; set; } = null!;
    [Export] public RigidBody2D RigidBody { get; set; } = null!;

    #endregion
}