using AngryBird.Globals;
using Godot;

namespace AngryBird;

[Tool]
public partial class ScoreItem : Node2D
{
    #region SizeScale

    private float _sizeScale = 1;

    [Export]
    public float SizeScale
    {
        get => _sizeScale;
        set => SetSizeScale(value);
    }

    private async void SetSizeScale(float value)
    {
        _sizeScale = value;
        await Helper.WaitNodeReady(this);
        Scale = Scale with { X = SizeScale, Y = SizeScale };
    }

    #endregion

    #region Score

    private int _score = 100;

    [Export]
    public int Score
    {
        get => _score;
        set => SetScore(value);
    }

    private async void SetScore(int value)
    {
        _score = value;
        await Helper.WaitNodeReady(this);
        ScoreLabel.Text = Score.ToString();
    }

    #endregion

    private ScoreItem()
    {
        SetSizeScale(1);
        SetScore(100);
    }

    public override void _Ready()
    {
        base._Ready();
        ItemArea.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Bird) return;
        Die();
    }

    private void Die()
    {
        Game.CurrentLevel.Score += Score;
        AnimationPlayer.Play("die");
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; } = null!;

    [Export] public Label ScoreLabel { get; set; } = null!;
    [Export] public Area2D ItemArea { get; set; } = null!;

    #endregion
}