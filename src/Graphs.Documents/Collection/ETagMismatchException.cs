using System;
using System.Runtime.Serialization;

namespace Graphs.Documents
{
    [Serializable]
    public class ETagMismatchException
        : Exception
    {
        public ETagMismatchException()
        {
        }

        public ETagMismatchException(string message) : base(message)
        {
        }

        protected ETagMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ETagMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
