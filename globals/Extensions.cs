using System.Threading.Tasks;
using AngryBird.Constants;
using Godot;

namespace AngryBird.Globals;

public static class Extensions
{
    public static async Task EnsureReadyAsync(this Node node)
    {
        if (!node.IsNodeReady())
            await node.ToSignal(node, Node.SignalName.Ready);
    }

    public static async Task DelayAsync(this Node node, float seconds)
    {
        var tree = node.GetTree();
        using var timer = tree.CreateTimer(seconds);
        await node.ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
    }

    public static async Task EnsureToolReadyAsync(this Node node, TaskCompletionSource toolReadyTCS)
    {
        if (!node.HasMeta(MetaNames.Reloading))
            node.SetMeta(MetaNames.Reloading, false);

        var isReloading = (bool)node.GetMeta(MetaNames.Reloading);

        if (isReloading)
            await toolReadyTCS.Task;
        else
            await node.EnsureReadyAsync();
    }
}