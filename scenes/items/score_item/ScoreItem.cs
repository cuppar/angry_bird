using System.Threading.Tasks;
using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird;

[Tool]
public partial class ScoreItem : Node2D, ISerializationListener
{
    private readonly TaskCompletionSource _toolReadyTCS = new();

    private ScoreItem()
    {
        SetSizeScale(1);
        SetScore(200);
    }

    #region ISerializationListener Members

    public void OnBeforeSerialize()
    {
        SetMeta(MetaNames.Reloading, true);
    }

    public void OnAfterDeserialize()
    {
        SetMeta(MetaNames.Reloading, false);
        _toolReadyTCS.SetResult();
    }

    #endregion

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
        await this.EnsureToolReadyAsync(_toolReadyTCS);
        _sizeScale = value;
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
        await this.EnsureToolReadyAsync(_toolReadyTCS);
        _score = value;
        ScoreLabel.Text = Score.ToString();
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; } = null!;

    [Export] public Label ScoreLabel { get; set; } = null!;
    [Export] public Area2D ItemArea { get; set; } = null!;

    #endregion
}