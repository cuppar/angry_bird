using Godot;

namespace AngryBird;

public partial class BirdInSlingshot : Area2D
{
    public bool IsMouseHover { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    private void OnMouseEntered()
    {
        IsMouseHover = true;
    }

    private void OnMouseExited()
    {
        IsMouseHover = false;
    }
}