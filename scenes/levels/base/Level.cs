using System.Linq;
using AngryBird.Constants;
using AngryBird.Globals;
using AngryBird.Globals.Extensions;
using AngryBird.UI;
using Godot;
using static System.Diagnostics.Debug;

namespace AngryBird;

public partial class Level : Node2D
{
    private Bird? _shotBird;

    public int Score { get; set; }

    public override void _Ready()
    {
        base._Ready();
        SetLiveLeft(LiveLeft);
        Game.CurrentLevel = this;
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
        ShotBirdContainer.AddChild(_shotBird);
        LiveLeft--;
    }

    private bool IsTurnStartedAndOvered()
    {
        if (!IsTurnStarted()) return false;
        Assert(_shotBird is not null, $"Turn started, {nameof(_shotBird)} should not be null");

        var birdSlept = _shotBird.IsSleeping();
        var birdOutOfScreen = !_shotBird.IsOnScreen();

        // pigs
        if (GetTree().GetNodesInGroup(Groups.Pigs).Any(p => !((RigidBody2D)p).IsSleeping()))
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
                GameOver();
            else
                SetupForNewTurn();
    }

    private void GameOver()
    {
        SetPhysicsProcess(false);
        GetTree().Paused = true;
        LevelFailPopup.PopupCentered();
    }

    private static bool IsLastUnlockedLevel()
    {
        return Game.UnlockedLevelCount == Game.CurrentLevelNumber;
    }

    private void GamePass()
    {
        if (IsLastUnlockedLevel())
            Game.UnlockedLevelCount += 1;

        SetPhysicsProcess(false);
        GetTree().Paused = true;
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
        await this.EnsureReadyAsync();
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
    [Export] public LevelFailPopup LevelFailPopup { get; set; } = null!;
    [Export] public Node2D ShotBirdContainer { get; set; } = null!;

    #endregion
}