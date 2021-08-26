using Graphs.Elements;
using System;

namespace Game.Controller.ActionHandlers
{
    internal interface IActionHandler
    {
        event EventHandler<ActionHandledEventArgs> ActionHandled;

        void HandleAction(IGraph graph, Node player, Command command);
    }
}
