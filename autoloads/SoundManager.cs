using Godot;

namespace AngryBird.Autoloads;

public partial class SoundManager : Node
{
    public override void _Ready()
    {
        base._Ready();
        ProcessMode = ProcessModeEnum.Always;
    }

    #region Bus enum

    public enum Bus
    {
        Master,
        SFX,
        BGM
    }

    #endregion

    private float _volume;

    public void PlaySFX(string sfxName)
    {
        var player = SFX.GetNode<AudioStreamPlayer>(sfxName);
        player?.Play();
    }

    public void SetupUISounds(Node node)
    {
        switch (node)
        {
            case TextureButton textureButton:
                textureButton.Pressed += () => PlaySFX("UIPress");
                textureButton.FocusEntered += () => PlaySFX("UIFocus");
                textureButton.MouseEntered += textureButton.GrabFocus;
                break;
            case Button button:
                button.Pressed += () => PlaySFX("UIPress");
                button.FocusEntered += () => PlaySFX("UIFocus");
                button.MouseEntered += button.GrabFocus;
                break;
            case Slider slider:
                slider.ValueChanged += _ => PlaySFX("UIPress");
                slider.FocusEntered += () => PlaySFX("UIFocus");
                slider.MouseEntered += slider.GrabFocus;
                break;
        }

        foreach (var child in node.GetChildren())
            SetupUISounds(child);
    }

    public void PlayBGM(AudioStream audioStream)
    {
        if (BGMPlayer.Stream == audioStream && BGMPlayer.Playing)
            return;
        BGMPlayer.Stream = audioStream;
        BGMPlayer.Play();
    }

    public static float GetVolume(Bus bus)
    {
        var db = AudioServer.GetBusVolumeDb((int)bus);
        return Mathf.DbToLinear(db);
    }

    public static void SetVolume(Bus bus, float volume)
    {
        var db = Mathf.LinearToDb(volume);
        AudioServer.SetBusVolumeDb((int)bus, db);
    }


    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    public Node SFX = null!;

    [Export] public AudioStreamPlayer BGMPlayer = null!;

    #endregion
}