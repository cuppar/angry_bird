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
}