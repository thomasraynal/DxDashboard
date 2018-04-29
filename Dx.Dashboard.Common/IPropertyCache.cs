using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public interface IPropertyCache
    {
        bool IsInitialized { get; }
        Task Initialize(ViewModelBase model);
        T GetOrCreate<T>(T defaultValue, [CallerMemberName]  string key = null);
        void SetOrCreate<T>(T value, [CallerMemberName]  string key = null);
    }
}
