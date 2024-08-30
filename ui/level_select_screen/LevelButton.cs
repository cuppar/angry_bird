using AngryBird.Autoload;
using AngryBird.Classes;
using AngryBird.Constants;
using Godot;

namespace AngryBird.UI;

public partial class LevelButton : ImageButton
{
    private int _level = 1;

    private bool _locked = true;

    [Export]
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            LevelLabel.Text = Level.ToString();
        }
    }

    [Export]
    public bool Locked
    {
        get => _locked;
        set
        {
            _locked = value;
            if (_locked)
                Lock();
            else
                Unlock();
        }
    }


    public override void _Ready()
    {
        base._Ready();
        Locked = true;
        Level = 1;
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        AutoloadManager.SceneTranslation.ChangeSceneToFile(ScenePaths.GetLevel(Level));
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


    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    public Label LevelLabel = null!;

    [Export] public Control LockTexture = null!;

    #endregion
}