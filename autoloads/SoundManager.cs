using AngryBird.Constants;
using Godot;

namespace AngryBird.Autoloads;

public partial class SoundManager : Node
{
    #region Bus enum

    public enum Bus
    {
        Master,
        SFX,
        BGM
    }

    #endregion

    private float _volume;

    public override void _Ready()
    {
        base._Ready();
        ProcessMode = ProcessModeEnum.Always;
    }

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
                textureButton.Pressed += () => PlaySFX(SFXNames.UIPress);
                textureButton.FocusEntered += () => PlaySFX(SFXNames.UIFocus);
                textureButton.MouseEntered += textureButton.GrabFocus;
                break;
            case Button button:
                button.Pressed += () => PlaySFX(SFXNames.UIPress);
                button.FocusEntered += () => PlaySFX(SFXNames.UIFocus);
                button.MouseEntered += button.GrabFocus;
                break;
            case Slider slider:
                slider.ValueChanged += _ => PlaySFX(SFXNames.UIPress);
                slider.FocusEntered += () => PlaySFX(SFXNames.UIFocus);
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