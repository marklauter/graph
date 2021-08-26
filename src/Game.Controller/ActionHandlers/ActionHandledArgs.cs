using System;

namespace Game.Controller.ActionHandlers
{
    internal sealed class ActionHandledEventArgs
        : EventArgs
    {
        public ActionHandledEventArgs(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            }

            this.Message = message;
        }

        public string Message { get; }
    }
}
