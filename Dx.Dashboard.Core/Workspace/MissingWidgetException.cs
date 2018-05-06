using System;
using System.Runtime.Serialization;

namespace Dx.Dashboard.Core
{
    [Serializable]
    internal class MissingWidgetException : Exception
    {
        public MissingWidgetException()
        {
        }

        public MissingWidgetException(string message) : base(String.Format($"Unable to find widget {message}"))
        {
        }

        public MissingWidgetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingWidgetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}