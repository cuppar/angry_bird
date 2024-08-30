using System;
using System.Linq;
using AngryBird.Autoloads;
using Godot;

namespace AngryBird.UI;

public partial class HUD : CanvasLayer
{
    private static readonly Type[] ScenesNeedHideHUD = { typeof(TitleScreen), typeof(LevelSelectScreen) };

    private void HideHUDInSomeScene()
    {
        AutoloadManager.SceneTranslation.AfterSceneChanged += (_, newScene) =>
        {
            Show();
            if (ScenesNeedHideHUD.Any(scene => newScene.GetType() == scene)) Hide();
        };
    }

    public override void _Ready()
    {
        base._Ready();
        Hide();
        HideHUDInSomeScene();
    }
}