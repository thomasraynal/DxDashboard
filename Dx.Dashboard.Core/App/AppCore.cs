using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public class AppCore
    {
        private static Lazy<IAppContainer> _lazyInstance;

        public static IAppContainer Instance
        {
            get
            {
                if (null == _lazyInstance)
                {

                    var app = AppContainer.Create();
                    _lazyInstance = new Lazy<IAppContainer>(() => app, LazyThreadSafetyMode.ExecutionAndPublication);
                }

                return _lazyInstance.Value;
            }
        }
    }
}
