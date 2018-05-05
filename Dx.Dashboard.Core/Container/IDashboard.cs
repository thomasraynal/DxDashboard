using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public interface IDashboard<TState> where TState : class, IWorkspaceState
    {
        IEnumerable<WidgetAttribute> AvailableWidgets { get; }
        ReactiveCommand CreateWidgetCommand { get; }
        Task<IWidget> CreateWidget(Type widget, String cacheId);
        ReactiveList<WorkspaceLayout> UserDefinedWorkspaceLayouts { get; set; }
        ReactiveList<IWorkspace<TState>> AvailableWorkspaces { get; set; }
        IWorkspace<TState> CurrentWorkspace { get; set; }
        TState GetState();
        bool HideMenus { get; set; }
        ReactiveList<IMenuItemDescriptor> MenuItems { get; set; }
        Task CreateWorkspace(TState name, bool loadLayout);
    }
}
