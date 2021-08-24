using Graphs.Elements;

namespace Game.Controller.ActionHandlers
{
    public interface IActionHandler
    {
        void HandleAction(IGraph graph, Node player, Node verb, Node target);
    }
}
