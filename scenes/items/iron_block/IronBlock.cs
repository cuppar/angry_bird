using Godot;

namespace AngryBird;

public partial class IronBlock : RigidBody2D
{
    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public AudioStreamPlayer2D ImpactSFX { get; set; } = null!;

    #endregion

    public override void _Ready()
    {
        base._Ready();
        MaxContactsReported = 8;
        ContactMonitor = true;
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        ImpactSFX.Play();
    }
}