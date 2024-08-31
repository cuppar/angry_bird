using AngryBird.Constants;
using Godot;

namespace AngryBird;

[GlobalClass]
public partial class Slingshot : Node2D
{
    [Export] public float MaxRadius = 150;
    [Export] public float MaxForce = 60000;

    [Signal]
    public delegate void ShootEventHandler(Bird bird);

    private PackedScene _birdPrefab = null!;

    public override void _Ready()
    {
        base._Ready();
        _birdPrefab = ResourceLoader.Load<PackedScene>(PrefabPaths.Character.Bird);
    }

    private bool _dragging;

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } eventMouseButton)
        {
            if (eventMouseButton.IsPressed())
            {
                if (!BirdInSlingshot.IsMouseHover)
                    return;
                GD.Print($"mouse down");
                _dragging = true;
            }
            else if (eventMouseButton.IsReleased() && _dragging)
            {
                GD.Print($"mouse up");
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
            GD.Print($"dragging");
            var mousePos = eventMouseMotion.Position - GlobalPosition;
            var distance = mousePos.Length();

            BirdInSlingshot.LookAt(GlobalPosition);
            if (distance < MaxRadius)
                BirdInSlingshot.Position = mousePos;
            else
            {
                var direction = (Vector2.Zero - mousePos).Normalized();
                BirdInSlingshot.Position = -direction * MaxRadius;
            }
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    public BirdInSlingshot BirdInSlingshot = null!;

    #endregion
}