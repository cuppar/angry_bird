using System.Threading.Tasks;
using Godot;

namespace AngryBird.Globals;

public static class Helper
{
    public static async Task WaitNodeReady(Node node)
    {
        if (!node.IsNodeReady())
            await node.ToSignal(node, Node.SignalName.Ready);
    }

    public static async Task WaitSeconds(float seconds)
    {
        var tree = (SceneTree)Engine.GetMainLoop();
        using var timer = tree.CreateTimer(seconds);
        await tree.ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
    }
}