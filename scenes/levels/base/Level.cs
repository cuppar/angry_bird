using Godot;
using static System.Diagnostics.Debug;

namespace AngryBird;

public partial class Level : Node2D
{
    private Bird? _shotBird;

    public override void _Ready()
    {
        base._Ready();
        Slingshot.Shoot += OnSlingshotShoot;
    }

    private void OnSlingshotShoot(Bird bird)
    {
        _shotBird = bird;
        Slingshot.ReadyToShoot = false;
        _shotBird.LeftLimit = LeftLimit;
        _shotBird.RightLimit = RightLimit;
        RemoveChild(Camera);
        _shotBird.AddChild(Camera);
        AddChild(_shotBird);
    }

    private bool IsTurnStartedAndOvered()
    {
        if (!IsTurnStarted()) return false;
        Assert(_shotBird is not null, $"Turn started, {nameof(_shotBird)} should not be null");

        var birdSlept = _shotBird.IsSleeping();
        var birdOutOfScreen = !_shotBird.IsOnScreen();

        if (birdOutOfScreen || birdSlept)
            return true;

        return false;
    }

    private bool IsTurnStarted()
    {
        return !Slingshot.ReadyToShoot;
    }

    private void SetupForNewTurn()
    {
        Slingshot.ReadyToShoot = true;
        Assert(_shotBird is not null, $"In {nameof(SetupForNewTurn)}, {nameof(_shotBird)} should not be null");

        _shotBird.RemoveChild(Camera);
        AddChild(Camera);
        _shotBird.QueueFree();
        _shotBird = null;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (IsTurnStartedAndOvered())
            SetupForNewTurn();
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Slingshot Slingshot { get; set; } = null!;

    [Export] public Camera Camera { get; set; } = null!;
    [Export] public Marker2D LeftLimit = null!;
    [Export] public Marker2D RightLimit = null!;

    #endregion
}