using AngryBird.Autoloads;
using AngryBird.Constants;
using AngryBird.Globals;
using AngryBird.Globals.Extensions;
using Godot;

namespace AngryBird;

[GlobalClass]
public partial class Slingshot : Node2D
{
    #region Delegates

    [Signal]
    public delegate void ShootEventHandler(Bird bird);

    #endregion

    private PackedScene _birdPrefab = null!;

    private bool _dragging;
    private PackedScene _trajectoryPointPrefab = null!;
    [Export] public float MaxForce = 1500;
    [Export] public float MaxRadius = 150;


    public override void _Ready()
    {
        base._Ready();
        _birdPrefab = ResourceLoader.Load<PackedScene>(PrefabPaths.Character.Bird);
        _trajectoryPointPrefab = ResourceLoader.Load<PackedScene>(PrefabPaths.UI.TrajectoryPoint);
        FrontLine.AddPoint(BirdInSlingshot.Position);
        BackLine.AddPoint(BirdInSlingshot.Position);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        var birdBackPos = BirdInSlingshot.Position -
                          (BirdInSlingshot.Transform.X - BirdInSlingshot.Transform.Y * (float)0.8) * 15;
        FrontLine.SetPointPosition(1, birdBackPos);
        BackLine.SetPointPosition(1, birdBackPos);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } eventMouseButton)
        {
            // mouse down
            if (eventMouseButton.IsPressed())
            {
                if (!BirdInSlingshot.IsMouseHover)
                    return;
                _dragging = true;
            }
            // mouse up
            else if (eventMouseButton.IsReleased() && _dragging)
            {
                _dragging = false;
                var bird = _birdPrefab.Instantiate<Bird>();
                bird.InitImpulse = GetShootInitImpulse();
                bird.GlobalPosition = BirdInSlingshot.GlobalPosition;
                EmitSignal(SignalName.Shoot, bird);
                BirdInSlingshot.Reset();
                ClearTrajectory();
            }
        }

        // dragging
        if (@event is InputEventMouseMotion eventMouseMotion && _dragging)
        {
            // 改变弹弓上小鸟位置
            var mousePos = eventMouseMotion.Position - GlobalPosition;
            var distance = mousePos.Length();

            BirdInSlingshot.LookAt(GlobalPosition);
            if (distance < MaxRadius)
            {
                BirdInSlingshot.Position = mousePos;
            }
            else
            {
                var direction = (Vector2.Zero - mousePos).Normalized();
                BirdInSlingshot.Position = -direction * MaxRadius;
            }

            // 简单模式显示轨迹
            if (Game.CurrentMode == Game.Mode.Easy)
                DrawTrajectory(BirdInSlingshot.GlobalPosition);

            // 满弓音效
            if (IsBigPower(distance))
            {
                if (!_isBigPowerInLastFrame) AutoloadManager.SoundManager.PlaySFX(SFXNames.SlingshotBigPower);

                _isBigPowerInLastFrame = true;
            }
            else
            {
                _isBigPowerInLastFrame = false;
            }
        }
    }


    private Vector2 GetShootInitImpulse()
    {
        var birdPos = BirdInSlingshot.Position;
        var direction = -birdPos.Normalized();
        var length = birdPos.Length();
        var force = length / MaxRadius * MaxForce;
        return force * direction;
    }

    private void DrawTrajectory(Vector2 startPosition)
    {
        const int trajectoryPointCount = 50;
        const float trajectoryPointStepX = 30;
        ClearTrajectory();
        for (var i = 0; i < trajectoryPointCount; i++)
        {
            var point = _trajectoryPointPrefab.Instantiate<Node2D>();
            TrajectoryContainer.AddChild(point);

            var x = i * trajectoryPointStepX;

            if (startPosition.X > GlobalPosition.X)
                x = -x;

            var offset = new Vector2(x, GetYByX(x));
            var pos = startPosition + offset;
            point.GlobalPosition = pos;
        }

        return;

        float GetYByX(float x)
        {
            var bird = _birdPrefab.Instantiate<Bird>();
            var mass = bird.Mass;
            var gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
            var initImpulse = GetShootInitImpulse();
            var velocity = initImpulse / mass;
            var delta = velocity.Angle();
            var velocityX = velocity.Length() * Mathf.Cos(delta);
            var velocityY = velocity.Length() * Mathf.Sin(delta);
            var y = velocityY * x / velocityX + 0.5f * gravity * x * x / (velocityX * velocityX);

            bird.QueueFree();
            return y;
        }
    }

    private void ClearTrajectory()
    {
        foreach (var child in TrajectoryContainer.GetChildren())
            if (child.Owner is null)
                child.QueueFree();
    }

    #region 是否接近拉满弓

    private bool _isBigPowerInLastFrame;
    private const float Factor = 0.8f;

    private bool IsBigPower(float distance)
    {
        return distance > MaxRadius * Factor;
    }

    #endregion

    #region ReadyToShoot

    private bool _readyToShoot = true;

    [Export]
    public bool ReadyToShoot
    {
        get => _readyToShoot;
        set => SetReadyToShoot(value);
    }

    private async void SetReadyToShoot(bool value)
    {
        _readyToShoot = value;
        await this.EnsureReadyAsync();
        if (_readyToShoot)
            BirdInSlingshot.Show();
        else
            BirdInSlingshot.Hide();
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    public BirdInSlingshot BirdInSlingshot = null!;

    [Export] public Line2D FrontLine = null!;
    [Export] public Line2D BackLine = null!;
    [Export] public Node2D TrajectoryContainer { get; set; } = null!;

    #endregion
}