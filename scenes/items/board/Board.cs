using AngryBird.Globals;
using Godot;

namespace AngryBird;

public partial class Board : RigidBody2D
{
    private const int ScoreFirstHit = 50;
    private const int ScoreSecondHit = 100;
    private int HitCount { get; set; }

    public override void _Ready()
    {
        base._Ready();
        ContactMonitor = true;
        MaxContactsReported = 8;
        BodyEntered += OnBodyEntered;
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
                // todo 木板音效1
                GD.Print($"木板音效1");
                Game.CurrentLevel.Score += ScoreFirstHit;
                OriginSprite.Hide();
                HitSprite.Show();
                break;
            case 2:
                // todo 木板音效2
                GD.Print($"木板音效2");
                Game.CurrentLevel.Score += ScoreSecondHit;
                QueueFree();
                break;
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D OriginSprite { get; set; } = null!;

    [Export] public Sprite2D HitSprite { get; set; } = null!;

    #endregion
}