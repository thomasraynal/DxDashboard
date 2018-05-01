using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dx.Dashboard;
using Dx.Dashboard.Core;

namespace Dx.Dashboard.Demo
{
    public class DemoWorkspaceState : IWorkspaceState
    {
        public DemoWorkspaceState(String name, DateTime date, DemoWorkspaceType type)
        {
            _defaultName = name;
            Date = date;
            Type = type;
        }

        public DemoWorkspaceType Type { get; private set; }

        private string _defaultName;
        public string Name
        {
            get
            {
                return Strategy == null ? _defaultName : Strategy.Name;
            }
        }

        public DateTime Date { get; private set; }
        public IStrategy Strategy { get; internal set; }

        public string Tag => Type.ToString();

        public bool Equals(IWorkspaceState other)
        {
            return this.Name == other.Name;
        }
    }
}
