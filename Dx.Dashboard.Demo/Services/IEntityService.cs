using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public interface IEntityService<TEntity>
    {
        IObservableCache<TEntity, String> All { get; }
        Task<bool> CreateOrUpdate(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }
}
