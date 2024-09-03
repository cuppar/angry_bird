using System.Linq;
using AngryBird.Constants;
using AngryBird.Globals;
using AngryBird.UI;
using Godot;
using static System.Diagnostics.Debug;

namespace AngryBird;

public partial class Level : Node2D
{
    private Bird? _shotBird;

    public override void _Ready()
    {
        base._Ready();
        SetLiveLeft(LiveLeft);
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
        LiveLeft--;
    }

    private bool IsTurnStartedAndOvered()
    {
        if (!IsTurnStarted()) return false;
        Assert(_shotBird is not null, $"Turn started, {nameof(_shotBird)} should not be null");

        var birdSlept = _shotBird.IsSleeping();
        var birdOutOfScreen = !_shotBird.IsOnScreen();

        // pigs
        if (GetTree().GetNodesInGroup(Groups.Pigs).Any(p => !((Pig)p).IsSleeping()))
            return false;

        // bird
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

    private bool IsGameOver()
    {
        return LiveLeft <= 0;
    }

    private bool IsGamePass()
    {
        return !GetTree().GetNodesInGroup(Groups.Pigs).Any();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (IsGamePass())
            GamePass();
        else if (IsTurnStartedAndOvered())
            if (IsGameOver())
                GD.Print("Game Over");
            else
                SetupForNewTurn();
    }

    private void GamePass()
    {
        if (Game.UnlockedLevelCount == Game.CurrentLevel)
            Game.UnlockedLevelCount += 1;
        SetPhysicsProcess(false);
        LevelPassPopup.PopupCentered();
    }

    #region LiveLeft

    private int _liveLeft = 5;

    [Export]
    public int LiveLeft
    {
        get => _liveLeft;
        set => SetLiveLeft(value);
    }

    private async void SetLiveLeft(int value)
    {
        _liveLeft = value;
        await Helper.WaitNodeReady(this);
        LiveUI.LiveLeft = LiveLeft;
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Slingshot Slingshot { get; set; } = null!;

    [Export] public Camera Camera { get; set; } = null!;
    [Export] public Marker2D LeftLimit = null!;
    [Export] public Marker2D RightLimit = null!;
    [Export] public Live LiveUI { get; set; } = null!;
    [Export] public LevelPassPopup LevelPassPopup { get; set; } = null!;

    #endregion
}