using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DevExpress.Xpf.Docking;
using System.Windows;
using ReactiveUI;
using Dx.Dashboard.Core;
using Dx.Dashboard.Common;
using Dx.Dashboard.Common.Configuration;

namespace Dx.Dashboard.Core
{

    public abstract class WidgetViewModel<TUserControl, TWorkspace, TState> : ViewModelBase, IWidget, IMVVMDockingProperties
        where TUserControl : UserControl, new()
        where TWorkspace : IWorkspace<TState>
        where TState : class,IWorkspaceState
    {

        public IDashboardConfiguration Configuration
        {
            get
            {
                return AppContainer.Get<IDashboardConfiguration>();
            }
        }

        public List<String> GetWidgetConfigurationItemArray(String key)
        {
            return Configuration.GetWidgetConfigurationItemArray(Name, key);
        }

        public String GetWidgetConfigurationKey(String key)
        {
            return Configuration.GetWidgetConfigurationKey(Name, key);
        }

        private UserControl _view;
        public UserControl View
        {
            get
            {
                return _view;
            }
        }

        public string TargetName
        {
            get
            {
                return "WidgetsViewHost";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                var widget = this.GetType().GetCustomAttributes(true).FirstOrDefault(attribute => (attribute as WidgetAttribute) != null);
                if (null == widget) throw new WidgetAttributeNotFound(this.GetType());
                return (widget as WidgetAttribute).Name;
            }
        }

        public String Header
        {
            get { return PropertyCache.GetOrCreate(Name); }
            set { PropertyCache.SetOrCreate(value); }
        }

        private String _parentName;
        public String ParentName
        {
            get { return _parentName; }
            set { _parentName = value; }
        }

        public String DockId { get; set; }
        public List<GridLayoutItem> GridsLayout { get; set; }
        public List<GridLayoutItem> PivotGridsLayout { get; set; }

        public Command DockLayoutContainerCloseCommand { get; private set; }

        private TWorkspace _host;
        public TWorkspace Workspace
        {
            get
            {
                return _host;
            }
        }


        public bool IsFloating
        {
            get
            {
                return this.View.Parent == null;
            }
        }

        public virtual bool AllowGridSaveLayout { get; protected set; }

        public string ViewModelId { get; private set; }

        public WidgetViewModel(TWorkspace host, String cacheId) : base(cacheId)
        {
            _view = new TUserControl();
            _host = host;
            ParentName = "_" + new String(Guid.NewGuid().ToString().Replace("-", "").Take(6).ToArray());
            GridsLayout = new List<GridLayoutItem>();
            PivotGridsLayout = new List<GridLayoutItem>();
            AllowGridSaveLayout = true;
            DockId = Guid.NewGuid().ToString();

            DockLayoutContainerCloseCommand = new Command(() =>
            {
                Workspace.Widgets.Remove(this);
            });
        }

        public WidgetDescriptor GetDescriptor()
        {
            return new WidgetDescriptor()
            {
                DockId = DockId,
                ViewModelId = ViewModelId,
                IsFloating = IsFloating,
                ParentName = ParentName,
                Type = GetType().AssemblyQualifiedName,
                GridsLayout = GridsLayout,
                PivotGridsLayout = PivotGridsLayout
            };
        }
    }
}
