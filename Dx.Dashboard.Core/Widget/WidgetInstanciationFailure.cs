using DevExpress.Utils.Serializing.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{

    [Serializable]
    public class WidgetInstanciationFailure : Exception
    {
        public WidgetInstanciationFailure(string message)
            : base(message)
        {
        }

        public WidgetInstanciationFailure(string message, Exception innerException)
        : base(message, innerException)
        { }

        protected WidgetInstanciationFailure(System.Runtime.Serialization.SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        { }
    }

}
