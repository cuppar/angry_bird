using Godot;

namespace AngryBird;

public partial class Level : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        Slingshot.Shoot += bird => { AddChild(bird); };
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Slingshot Slingshot { get; set; } = null!;

    #endregion
}