﻿using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public class AppContainer : IAppContainer
    {

        private IContainer _container;

        public static IAppContainer Create()
        {
            var container = new Container(x => x.AddRegistry<AppCoreRegistry>());

            var instance = container.GetInstance<AppContainer>();

            return instance;
        }

        public AppContainer(IContainer container)
        {
            _container = container;
        }

        public T Get<T>()
        {
            return _container.TryGetInstance<T>();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _container.GetAllInstances<T>();
        }

        public IContainer ObjectProvider
        {
            get
            {
                return _container;
            }
        }
    }
}
