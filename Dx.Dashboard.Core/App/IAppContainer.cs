using StructureMap;
using System.Collections.Generic;

namespace Dx.Dashboard.Core
{
    public interface IAppContainer
    {
        IContainer ObjectProvider { get; }
        T Get<T>();
        IEnumerable<T> GetAll<T>();
    }
}