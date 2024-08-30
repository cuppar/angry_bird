using AngryBird.Globals;
using Godot;

public partial class Test : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        GetNode<Label>("Label").Text = $"Game mode: {Game.CurrentMode}";
    }
}