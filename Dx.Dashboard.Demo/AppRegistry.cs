using Akavache;
using Dx.Dashboard.Cache;
using Dx.Dashboard.Common;
using Dx.Dashboard.Common.Cache;
using Dx.Dashboard.Common.Configuration;
using Dx.Dashboard.Common.Infra;
using Dx.Dashboard.Core;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Dx.Dashboard.Demo
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            For<IClientLogger>().Use<NLogger>().Singleton();
            For<ISchedulerProvider>().Use<DefaultSchedulerProvider>().Singleton();
            For<IExceptionHandler>().Use<DefaultExceptionHandler>();
            For<IPropertyCache>().Use<ReactiveSettingsStorage>();
            For<IBlobCache>().Use(() => BlobCache.UserAccount);

            For<IDashboard<DemoWorkspaceState>>().Use<DashboardViewModel>().Singleton();
            For<IWorkspace<DemoWorkspaceState>>().Use<DemoWorkspaceViewModel>();
            //Forward<DashboardViewModel, IDashboard<DemoWorkspaceState>>();
            For<IDashboardConfiguration>().Use<DemoConfiguration>().Singleton();

            For<WindowFactory>().Use<WindowFactory>().Singleton();
            For<Dispatcher>().Add(Application.Current.Dispatcher);

            For<ICacheStrategy<List<WorkspaceLayout>>>().Use<DefaultCacheStrategy<List<WorkspaceLayout>>>().Singleton();
            For<ICacheStrategy<WorkspaceLayout>>().Use<DefaultCacheStrategy<WorkspaceLayout>>().Singleton();

            For<IStrategy>().Use<Strategy>();
            For<ITrade>().Use<Trade>();

  
            For<IEntityService<IPrice>>().Use<PriceService>().Singleton();
            For<IEntityService<IStrategy>>().Use<StrategyService>().Singleton();

            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();

                scanner.WithDefaultConventions();

                //scanner.LookForRegistries();
            });
        }
    }
}
