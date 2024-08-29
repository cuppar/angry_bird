using Godot;

namespace AngryBird.Classes;

[GlobalClass]
public partial class HurtBox : Area2D
{
    #region Delegates

    [Signal]
    public delegate void HurtEventHandler(HitBox hitBox);

    #endregion
}