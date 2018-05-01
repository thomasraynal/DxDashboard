using Dx.Dashboard.Common;
using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dx.Dashboard.Demo
{
    public abstract class DemoWidgetViewModelBase<TUserControl> : WidgetViewModel<TUserControl, DemoWorkspaceViewModel, DemoWorkspaceState>
         where TUserControl : UserControl, new()
    {
        public IEntityService<IStrategy> StrategyService
        {
            get
            {
                return AppCore.Instance.Get<IEntityService<IStrategy>>();
            }
        }

        public IEntityService<IPrice> PriceService
        {
            get
            {
                return AppCore.Instance.Get<IEntityService<IPrice>>();
            }
        }

        public DemoWidgetViewModelBase(DemoWorkspaceViewModel host, String cacheID = null) : base(host, cacheID)
        {
        }

    }
}
