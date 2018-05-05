using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Common
{
    public abstract class PropertyCacheBase : ReactiveObject, IPropertyCache
    {
        protected String _cacheKey;

        private bool _isInitialized;
        protected ViewModelBase _model;

        public bool IsInitialized => _isInitialized;

        public abstract T GetOrCreate<T>(T defaultValue, [CallerMemberName] string key = null);

        protected virtual Task InitializeInternal()
        {
            return Task.CompletedTask;
        }

        public async Task Initialize(ViewModelBase model)
        {
            _cacheKey = model.ViewModelId;
            _model = model;
            await InitializeInternal();
            _isInitialized = true;
        }

        public abstract void SetOrCreate<T>(T value, [CallerMemberName] string key = null);

    }
}
