using StructureMap;
using System.Collections.Generic;

namespace Dx.Dashboard.Common
{
    public interface IAppContainer
    {
        IContainer ObjectProvider { get; }
        T Get<T>();
        IEnumerable<T> GetAll<T>();
    }
}