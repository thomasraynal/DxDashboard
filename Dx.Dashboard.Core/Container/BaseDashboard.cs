﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using DevExpress.Xpf.Bars;
using System.Windows.Media;
using Dx.Dashboard.Core;
using StructureMap.Pipeline;
using Dx.Dashboard.Common;
using Dx.Dashboard.Cache;
using Dx.Dashboard.Core.Extensions;

namespace Dx.Dashboard.Core
{
    public abstract class BaseDashboard<TState> : ViewModelBase, IDashboard<TState> where TState : class, IWorkspaceState
    {
   
        public ReactiveCommand CreateNewWidget { get; private set; }

        public ICacheStrategy<List<WorkspaceLayout>> Cache
        {
            get
            {
                return AppCore.Instance.Get<ICacheStrategy<List<WorkspaceLayout>>>();
            }
        }

        public ReactiveList<IMenuItemDescriptor> _dashboardMenu;
        public ReactiveList<IMenuItemDescriptor> DashboardMenu
        {
            get { return _dashboardMenu; }
            set { this.RaiseAndSetIfChanged(ref _dashboardMenu, value); }
        }

        public ReactiveList<WorkspaceLayout> _userDefinedWorkspaceLayouts;
        public ReactiveList<WorkspaceLayout> UserDefinedWorkspaceLayouts
        {
            get { return _userDefinedWorkspaceLayouts; }
            set { this.RaiseAndSetIfChanged(ref _userDefinedWorkspaceLayouts, value); }
        }

        private ReactiveList<IWorkspace<TState>> _workspaces;
        public ReactiveList<IWorkspace<TState>> AvailableWorkspaces
        {
            get { return _workspaces; }
            set { _workspaces = value; }
        }

        public IWorkspace<TState> _selectedWorkspace;
        public IWorkspace<TState> CurrentWorkspace
        {
            get { return _selectedWorkspace; }
            set { this.RaiseAndSetIfChanged(ref _selectedWorkspace, value); }
        }

        IEnumerable<WidgetAttribute> _availableWidgets;
        public IEnumerable<WidgetAttribute> AvailableWidgets
        {
            get
            {
                return _availableWidgets;
            }
        }

        public async virtual Task<IWidget> CreateWidget(Type widget, String cacheId = null)
        {
            var args = new ExplicitArguments();

            var id = cacheId ?? Guid.NewGuid().ToString();

            args.SetArg("host",CurrentWorkspace);
            args.Set(id);

            var instance =  AppContainer.ObjectProvider.GetInstance(widget, args) as IWidget;

            if (null == instance) return null;

            await instance.Initialize();

            return instance;
        }

        public BaseDashboard()
        {
            DashboardMenu = new ReactiveList<IMenuItemDescriptor>();

            AvailableWorkspaces = new ReactiveList<IWorkspace<TState>>();

            UserDefinedWorkspaceLayouts = new ReactiveList<WorkspaceLayout>(Cache.PersistantCache.Get(DxDashboardConstants.userDefinedLayouts).Result);

            _availableWidgets = AppContainer.ObjectProvider.Model.AllInstances
             .Where(instance => instance.ReturnedType.GetCustomAttributes(true).Any(attribute => typeof(WidgetAttribute).IsAssignableFrom(attribute.GetType())))
             .Select(widget => widget.ReturnedType.GetCustomAttributes(true).Where(attribute => typeof(WidgetAttribute).IsAssignableFrom(attribute.GetType())).First() as WidgetAttribute)
             .Distinct();

            this.WhenAnyObservable(vm => vm.AvailableWorkspaces.ItemsAdded).Subscribe(workspace =>
             {
                 CurrentWorkspace = workspace;
             });

            this.WhenAnyObservable(vm => vm.AvailableWorkspaces.ItemsRemoved)
                .Subscribe(obs =>
                {
                    obs.Widgets.ForEach(widget => widget.Dispose());
                });


            this.WhenAnyObservable(vm => vm.UserDefinedWorkspaceLayouts.ItemsAdded, vm => vm.UserDefinedWorkspaceLayouts.ItemsRemoved)
                .Subscribe(obs =>
                {
                    Cache.PersistantCache.Put(DxDashboardConstants.userDefinedLayouts, UserDefinedWorkspaceLayouts.ToList());
                });


            CreateNewWidget = ReactiveCommand.Create<String>(async (name) =>
            {
                var widgets = AvailableWidgets.First(widget => widget.Name == name);

                var @ref = AppContainer.ObjectProvider.Model.AllInstances
                .First(i => i.ReturnedType.GetCustomAttributes(true).Any(attribute => typeof(WidgetAttribute).IsAssignableFrom(attribute.GetType()) && (attribute as WidgetAttribute).Name ==  name));
                
                var instance = await CreateWidget(@ref.ReturnedType);
                
                CurrentWorkspace.Widgets.Add(instance);

            });

        }

        protected ListItemDescriptor CreateWidgetMenu(String menuTitle= "Widgets", ImageSource glyph = null)
        {

            if (null == glyph) glyph = DevExpressHelper.GetGlyph("GlobalColorScheme_16x16.png");

            var menu = new ListItemDescriptor(menuTitle, glyph);

            foreach (var item in AvailableWidgets)
            {
                var root = menu;

                foreach (var category in item.Category.Path.Split(';'))
                {
                    var match = root.Items.FirstOrDefault(menuItem => menuItem.Caption == category);

                    if (null == match)
                    {
                        var categoryMenu = new ListItemDescriptor(category, item.Glyph);
                        root.Items.Add(categoryMenu);
                        root = categoryMenu;
                    }
                    else
                    {
                        root = match as ListItemDescriptor;
                    }
                }

                root.Items.Add((new WidgetButtonItemDescriptor(item.Name, item, item.Glyph) { Command = CreateNewWidget }));

            }

            return menu;
        }

        public async Task CreateWorkspace(TState state, bool loadLayout)
        {

            var candidate = AvailableWorkspaces.FirstOrDefault(wspace => wspace.State.Equals(state));

            //we infer !loadLayout == create blank workspace
            if (null != candidate && loadLayout)
            {
                CurrentWorkspace = candidate;
                return;
            }

            var arg = new ExplicitArguments();
            arg.SetArg("loadLayout", loadLayout);

            var workspace = AppContainer.ObjectProvider.GetInstance<IWorkspace<TState>>(arg);

            await workspace.Initialize(state);

            AvailableWorkspaces.Add(workspace);

        }

        public abstract TState GetState();
    }

}

