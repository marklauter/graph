using Graphs.Elements;
using System;

namespace Game.Controller.ActionHandlers
{
    public interface IActionHandler
    {
        event EventHandler<ActionHandledEventArgs> ActionHandled;

        void HandleAction(IGraph graph, Node player, Node verb, Node target);
    }
}
