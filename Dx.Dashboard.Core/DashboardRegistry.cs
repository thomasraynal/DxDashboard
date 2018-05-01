using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Dx.Dashboard.Core
{
    public class DashboardRegistry : Registry
    {
        public DashboardRegistry()
        {
            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.AddAllTypesOf<IWidget>();
            });
        }

    }

}
