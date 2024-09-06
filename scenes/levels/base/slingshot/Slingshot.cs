using AngryBird.Constants;
using AngryBird.Globals;
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
    [Export] public float MaxForce = 100000;
    [Export] public float MaxRadius = 150;


    public override void _Ready()
    {
        base._Ready();
        _birdPrefab = ResourceLoader.Load<PackedScene>(PrefabPaths.Character.Bird);
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
            if (eventMouseButton.IsPressed())
            {
                if (!BirdInSlingshot.IsMouseHover)
                    return;
                // GD.Print("mouse down");
                _dragging = true;
            }
            else if (eventMouseButton.IsReleased() && _dragging)
            {
                // GD.Print("mouse up");
                _dragging = false;
                var birdPos = BirdInSlingshot.Position;
                var direction = -birdPos.Normalized();
                var length = birdPos.Length();
                var force = length / MaxRadius * MaxForce;
                var bird = _birdPrefab.Instantiate<Bird>();
                bird.InitForce = force * direction;
                bird.GlobalPosition = birdPos + GlobalPosition;
                EmitSignal(SignalName.Shoot, bird);
                BirdInSlingshot.Position = Vector2.Zero;
                BirdInSlingshot.Rotation = 0;
            }
        }

        if (@event is InputEventMouseMotion eventMouseMotion && _dragging)
        {
            // GD.Print("dragging");
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
        }
    }

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

    #endregion
}