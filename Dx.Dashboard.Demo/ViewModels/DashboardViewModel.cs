using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using Dx.Dashboard;
using Dx.Dashboard.Core;
using Dx.Dashboard.Common.Configuration;
using Dx.Dashboard.Core.Extensions;

namespace Dx.Dashboard.Demo
{
    public class DashboardViewModel : BaseDashboard<DemoWorkspaceState>
    {

        private ListItemDescriptor _widgetMenu;

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { this.RaiseAndSetIfChanged(ref _selectedDate, value); }
        }

        private ReactiveList<DateTime> _availableDates;
        public ReactiveList<DateTime> AvailableDates
        {
            get { return _availableDates; }
            set { this.RaiseAndSetIfChanged(ref _availableDates, value); }
        }

        protected async override Task InitializeInternal()
        {
            var state = new DemoWorkspaceState("PnL View", SelectedDate, DemoWorkspaceType.PnL);
            await CreateWorkspace(state, true);
        }

        public override DemoWorkspaceState GetState()
        {
            return new DemoWorkspaceState(CurrentWorkspace.State.Name, SelectedDate, CurrentWorkspace.State.Type);
        }

        public DashboardViewModel(IDashboardConfiguration config, IEntityService<IPrice> priceService, IEntityService<IStrategy> strategyService)
        {
            var dummyCommand = ReactiveCommand.Create(()=> { });

            AvailableDates = new ReactiveList<DateTime>(Enumerable.Range(0, 5).Select(offset => DateTime.Now.AddDays(-offset)));
            SelectedDate = AvailableDates.First();

            DashboardMenu.Add(new StaticItemDescriptor(String.Format("{0} - {1}", config.ApplicationName, config.Version)));

            _widgetMenu = CreateWidgetMenu();

            DashboardMenu.Add(_widgetMenu);

            this.WhenAnyValue(vm => vm.CurrentWorkspace)
                .Where(val => null != val)
                .Subscribe(obs =>
                {
                    _widgetMenu.GetChildrenMenuItems((item => item.GetType() == typeof(WidgetButtonItemDescriptor))).ForEach(item =>
                      {
                          item.IsEnabled = ((item as WidgetButtonItemDescriptor).Widget as WorkspaceAwareWidgetAttribute).IsVisible(obs.State);
                      });
                });

        }

    }
}
