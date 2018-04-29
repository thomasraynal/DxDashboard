using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Cache
{
    public interface ICacheStrategy<TObject>
    {
        ICacheProvider<TObject> PersistantCache { get; }
        ICacheProvider<TObject> MemoryCache { get; }
    }
}
