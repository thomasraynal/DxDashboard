using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive.Linq;
using Dx.Dashboard.Core;
using Dx.Dashboard.Cache;

namespace Dx.Dashboard.Demo
{
    public class DemoWorkspaceViewModel : BaseWorkspace<DemoWorkspaceState>
    {
        private DemoWorkspaceType _workspaceType;
        public DemoWorkspaceType WorkspaceType
        {
            get { return _workspaceType; }
            set { this.RaiseAndSetIfChanged(ref _workspaceType, value); }
        }

        public DemoWorkspaceViewModel(bool loadLayout = true) : base(loadLayout)
        {
        }

        protected override Task InitializeWorkspaceInternal()
        {
            Header = String.Format("{0} - {1}", State.Strategy == null? State.Name: State.Strategy.Name, State.Date.ToShortDateString());
            return Task.Delay(0);
        }
    }

}
