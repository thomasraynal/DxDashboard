using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Common
{
    public class AppCoreRegistry : Registry
    {
        public AppCoreRegistry()
        {
            For<IAppContainer>().Use<AppContainer>().Singleton();

            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.LookForRegistries();
                scanner.WithDefaultConventions();
            });
        }
    }
}

