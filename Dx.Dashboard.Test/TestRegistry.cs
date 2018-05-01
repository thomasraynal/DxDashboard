using Akavache;
using Dx.Dashboard.Cache;
using Dx.Dashboard.Common;
using Dx.Dashboard.Common.Configuration;
using Dx.Dashboard.Common.Infra;
using Dx.Dashboard.Core;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dx.Dashboard.Test
{

    public class ReactiveSettingsStorageProxy : ReactiveSettingsStorage
    {
        public ReactiveSettingsStorageProxy(IDashboardConfiguration configuration)
            : base(configuration)
        { }

        public T GetOrCreateProxy<T>(T defaultValue, string key)
        {
            return this.GetOrCreate(defaultValue, key);
        }

        public void SetOrCreateProxy<T>(T value, string key)
        {
            this.SetOrCreate(value, key);
        }
    }

    public class TestRegistry : Registry
    {
        public TestRegistry()
        {
            For<IClientLogger>().Use<TraceLogger>().Singleton();
            For<Dispatcher>().Add(Dispatcher.CurrentDispatcher);
            For<IPropertyCache>().Use<ReactiveSettingsStorageProxy>();
            For<IBlobCache>().Use(() => BlobCache.InMemory);

            For<IDashboard<TestWorkspaceState>>().Use<TestDashboard>().Singleton();
            For<IWorkspace<TestWorkspaceState>>().Use<TestWorkspace>();
            For<IDashboardConfiguration>().Use<TestConfiguration>().Singleton();

            For<ICacheStrategy<List<WorkspaceLayout>>>().Use<DefaultCacheStrategy<List<WorkspaceLayout>>>().Singleton();
            For<ICacheStrategy<WorkspaceLayout>>().Use<DefaultCacheStrategy<WorkspaceLayout>>().Singleton();

            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.WithDefaultConventions();
            });
        }
    }
}
