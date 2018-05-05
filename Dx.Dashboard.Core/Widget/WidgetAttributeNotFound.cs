using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    [Serializable]
    public class WidgetAttributeNotFound : Exception
    {
        public WidgetAttributeNotFound(Type viewModel)
            : base(String.Format("Unable to find widget attribute on viewmodel {0}", viewModel))
        { }

        public WidgetAttributeNotFound(string message, Exception innerException)
        : base(message, innerException)
        { }

        protected WidgetAttributeNotFound(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        { }
    }
}
