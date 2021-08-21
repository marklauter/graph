using System;

namespace Game.Controller.Exceptions
{
    public sealed class ActionNotFoundException
        : Exception
    {
        public ActionNotFoundException(string message) : base(message) { }
    }
}
