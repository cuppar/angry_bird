using System;
using AngryBird.UI;
using Godot;

// ReSharper disable MemberHidesStaticFromOuterClass

namespace AngryBird.Autoload;

public static class AutoloadManager
{
    #region Nested type: SceneTranslation

    public static class SceneTranslation
    {
        private static CanvasLayer InternalAutoload { get; } =
            ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CanvasLayer>("/root/SceneTranslation");

        #region Properties Bindings

        public static string ThreadLoadSceneFile => (string)InternalAutoload.Get(PropertyName.ThreadLoadSceneFile);

        #endregion

        #region Nested type: MethodName

        private abstract class MethodName : CanvasLayer.MethodName
        {
            public static readonly StringName ChangeSceneToFile = "change_scene_to_file";
            public static readonly StringName ChangeSceneToFileAsync = "change_scene_to_file_async";
            public static readonly StringName ChangeSceneToFileWithPause = "change_scene_to_file_with_pause";
            public static readonly StringName ChangeSceneToFileWithPauseAsync = "change_scene_to_file_with_pause_async";
            public static readonly StringName ChangeSceneToPacked = "change_scene_to_packed";
            public static readonly StringName ChangeSceneToPackedWithPause = "change_scene_to_packed_with_pause";
        }

        #endregion

        #region Nested type: PropertyName

        private abstract class PropertyName : CanvasLayer.PropertyName
        {
            public static readonly StringName ThreadLoadSceneFile = "thread_load_scene_file";
        }

        #endregion

        #region Nested type: SignalName

        private abstract class SignalName : CanvasLayer.SignalName
        {
            public static readonly StringName BeforeSceneChanged = "before_scene_changed";
            public static readonly StringName AfterSceneChanged = "after_scene_changed";
        }

        #endregion

        #region Method Bindings

        public static void ChangeSceneToFile(string sceneFile)
        {
            InternalAutoload.Call(MethodName.ChangeSceneToFile, sceneFile);
        }

        public static void ChangeSceneToFileAsync(string sceneFile)
        {
            InternalAutoload.Call(MethodName.ChangeSceneToFileAsync, sceneFile);
        }

        public static void ChangeSceneToFileWithPause(string sceneFile)
        {
            InternalAutoload.Call(MethodName.ChangeSceneToFileWithPause, sceneFile);
        }

        public static void ChangeSceneToFileWithPauseAsync(string sceneFile)
        {
            InternalAutoload.Call(MethodName.ChangeSceneToFileWithPauseAsync, sceneFile);
        }

        public static void ChangeSceneToPacked(PackedScene packedScene)
        {
            InternalAutoload.Call(MethodName.ChangeSceneToPacked, packedScene);
        }

        public static void ChangeSceneToPackedWithPause(PackedScene packedScene)
        {
            InternalAutoload.Call(MethodName.ChangeSceneToPackedWithPause, packedScene);
        }

        #endregion


        #region Signal Bindings

        public static event Action<Node, Node>? BeforeSceneChanged
        {
            add
            {
                if (value != null) InternalAutoload.Connect(SignalName.BeforeSceneChanged, Callable.From(value));
            }
            remove
            {
                if (value != null) InternalAutoload.Disconnect(SignalName.BeforeSceneChanged, Callable.From(value));
            }
        }

        public static event Action<Node, Node>? AfterSceneChanged
        {
            add
            {
                if (value != null) InternalAutoload.Connect(SignalName.AfterSceneChanged, Callable.From(value));
            }
            remove
            {
                if (value != null) InternalAutoload.Disconnect(SignalName.AfterSceneChanged, Callable.From(value));
            }
        }

        #endregion
    }

    #endregion

    public static SoundManager SoundManager { get; } =
        ((SceneTree)Engine.GetMainLoop()).Root.GetNode<SoundManager>("/root/SoundManager");
    
    public static HUD HUD { get; } =
        ((SceneTree)Engine.GetMainLoop()).Root.GetNode<HUD>("/root/HUD");
}