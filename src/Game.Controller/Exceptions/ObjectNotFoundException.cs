using System;

namespace Game.Controller.Exceptions
{
    public sealed class ObjectNotFoundException
        : Exception
    {
        public ObjectNotFoundException(string message) : base(message) { }
    }
}
