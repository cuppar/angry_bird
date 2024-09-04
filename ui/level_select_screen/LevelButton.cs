using AngryBird.Autoloads;
using AngryBird.Classes;
using AngryBird.Constants;
using AngryBird.Globals;
using Godot;

namespace AngryBird.UI;

public partial class LevelButton : ImageButton
{
    private LevelButton()
    {
        Locked = true;
        Level = 1;
    }

    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        Game.CurrentLevelNumber = Level;
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.GetLevel(Game.CurrentLevelNumber));
    }

    private void Lock()
    {
        Disabled = true;
        LockTexture.Show();
        LevelLabel.Hide();
    }

    private void Unlock()
    {
        Disabled = false;
        LockTexture.Hide();
        LevelLabel.Show();
    }

    #region Level

    private int _level = 1;

    [Export]
    public int Level
    {
        get => _level;
        set => SetLevel(value);
    }

    private async void SetLevel(int value)
    {
        _level = value;
        await Helper.WaitNodeReady(this);
        LevelLabel.Text = Level.ToString();
    }

    #endregion

    #region Locked

    private bool _locked = true;

    [Export]
    public bool Locked
    {
        get => _locked;
        set => SetLocked(value);
    }

    private async void SetLocked(bool value)
    {
        _locked = value;
        await Helper.WaitNodeReady(this);
        if (_locked)
            Lock();
        else
            Unlock();
    }

    #endregion


    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    public Label LevelLabel = null!;

    [Export] public Control LockTexture = null!;

    #endregion
}