using System;

namespace Game.Controller.Exceptions
{
    public sealed class ActionTargetNotFoundException
        : Exception
    {
        public ActionTargetNotFoundException(string message) : base(message) { }
    }
}
