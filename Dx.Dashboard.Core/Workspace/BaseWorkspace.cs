using Dx.Dashboard.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.PivotGrid;
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;
using Dx.Dashboard.Common;
using Dx.Dashboard.Cache;

namespace Dx.Dashboard.Core
{
    public abstract class BaseWorkspace<TState> : ViewModelBase, IWorkspace<TState> where TState: class, IWorkspaceState
    {

        #region Constants

        private String layoutDefaultExt = ".layout";
        private String layoutDefaultFilter = "Layouts (.layout)|*.layout";

        #endregion

        #region Properties

        public String _layoutName;
        public String TaggedLayoutLabel
        {
            get { return _layoutName; }
            set { this.RaiseAndSetIfChanged(ref _layoutName, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isLoading, value);
            }
        }

        private Guid _id;
        public Guid WorkspaceId
        {
            get { return _id; }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }

        private String _header;
        public String Header
        {
            get { return _header; }
            set { this.RaiseAndSetIfChanged(ref _header, value); }
        }

        public IDashboard<TState> Dashboard
        {
            get { return AppCore.Instance.Get<IDashboard<TState>>(); }
        }

        private TState _state;
        public TState State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value); }
        }

        private SmartReactiveList<IWidget> _widgets;
        public SmartReactiveList<IWidget> Widgets
        {
            get { return _widgets; }
            set { this.RaiseAndSetIfChanged(ref _widgets, value); }
        }

        private ICacheStrategy<WorkspaceLayout> CacheStrategy =>  AppCore.Instance.Get<ICacheStrategy<WorkspaceLayout>>(); 

    #endregion

    #region Commands

    public ReactiveCommand SaveLayout { get; private set; }
        public ReactiveCommand SaveTaggedLayout { get; private set; }
        public ReactiveCommand DeleteTaggedLayout { get; private set; }
        public ReactiveCommand SaveTemplateLayout { get; private set; }
        public ReactiveCommand ClearCurrentLayout { get; private set; }
        public ReactiveCommand SaveLayoutAsFile { get; private set; }
        public ReactiveCommand LoadLayoutFromFile { get; private set; }
        public ReactiveCommand LoadTaggedLayout { get; private set; }
        public ReactiveCommand OpenNewWorkspace { get; private set; }
        public Workspace View { get; set; }

        private async Task SaveCurrentLayoutAsTemplate()
        {
            var layout = GetCurrentLayout();
            layout.Name = State.Tag;
            await CacheStrategy.PersistantCache.Put(String.Format("{0}{1}", DxDashboardConstants.layoutWidgetsTemplateKey, State.Tag), layout);
        }

        private async Task SaveCurrentLayoutAsDefault()
        {
            var layout = GetCurrentLayout();
            layout.Name = State.Name;
            await CacheStrategy.PersistantCache.Put(String.Format("{0}{1}", DxDashboardConstants.layoutWidgetsKey, State.Name), layout);
        }

        private void SaveCurrentLayoutAsTagged()
        {
            var layout = GetCurrentLayout();
            layout.Name = TaggedLayoutLabel;

            if (Dashboard.UserDefinedWorkspaceLayouts.Contains(layout)) Dashboard.UserDefinedWorkspaceLayouts.Remove(layout);
            Dashboard.UserDefinedWorkspaceLayouts.Add(layout);

            this.ExecuteOnCurrentDispatcher(() =>
            {
                TaggedLayoutLabel = string.Empty;
            });  
        }

        public WorkspaceLayout GetCurrentLayout()
        {
            var processedPanels = new List<LayoutPanel>();

            VisualTreeWalker<LayoutPanel>.Execute(View, SetNameOnPanel);

            View.WidgetDockManager.ClosedPanels.Clear();

            var layout = new WorkspaceLayout { Widgets = Widgets.Select(y => y.GetDescriptor()).ToList() };

            var layoutId = DateTime.Now.Ticks;

            VisualTreeWalker<LayoutPanel>.Execute(View, layoutPanel =>
            {
                var widget = layoutPanel.DataContext as IWidget;

                if (null == widget) return;

                if (processedPanels.Contains(layoutPanel)) return;
                processedPanels.Add(layoutPanel);

                layoutPanel.Name = widget.ParentName;

                if (widget.AllowGridSaveLayout)
                {
                    VisualTreeWalker<GridControl>.Execute(widget.View, b =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            b.SaveLayoutToStream(ms);
                            ms.Position = 0;

                            widget.GridsLayout.RemoveAll(y => y.GridLayoutId != layoutId);

                            widget.GridsLayout.Add(new GridLayoutItem()
                            {
                                GridLayoutId = layoutId,
                                Layout = ms.ToArray(),
                                WidgetId = widget.DockId
                            }
                            );
                        }
                    });

                    VisualTreeWalker<PivotGridControl>.Execute(widget.View, b =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            b.SaveLayoutToStream(ms);
                            ms.Position = 0;

                            widget.PivotGridsLayout.RemoveAll(y => y.GridLayoutId != layoutId);

                            widget.PivotGridsLayout.Add(new GridLayoutItem()
                            {
                                GridLayoutId = layoutId,
                                Layout = ms.ToArray(),
                                WidgetId = widget.DockId
                            });

                        }
                    });
                }
            });

            SaveFloatingWidgetsGridLayouts();

            using (var ms = new MemoryStream())
            {
                View.WidgetDockManager.SaveLayoutToStream(ms);
                ms.Position = 0;
                layout.DockingLayout = ms.ToArray();
            }

            return layout;
        }

        #endregion

        #region Equality

        public override bool Equals(object obj)
        {
            IWorkspace<TState> tab;

            if ((tab = (obj as IWorkspace<TState>)) == null)
                return false;

            return tab.WorkspaceId == this.WorkspaceId;
        }

        public override int GetHashCode()
        {
            return WorkspaceId.GetHashCode();
        }

        #endregion

        #region Initialization

        protected abstract Task InitializeWorkspaceInternal();

        public async Task Initialize(TState state)
        {
            State = state;
            Header = state.Name;
            await InitializeWorkspaceInternal();
        }

        #endregion

        public BaseWorkspace(bool loadLayout = true)
        {

            _id = Guid.NewGuid();

            //This whole code is ugly... but mandatory... Binding With DockManager ItemsSource is lost when restoring the widgets.
            {

                this.ExecuteOnCurrentDispatcher(() =>
                {
                    if (null == View) return;

                    View.WidgetDockManager.DockItemClosed += (sender, e) =>
                    {
                        var widget = e.Item.DataContext as IWidget;

                        if (null == widget) return;

                        this.ExecuteOnCurrentDispatcher(() =>
                        {
                            widget.Dispose();
                            Widgets.Remove(widget);
                        });

                    };

                });

                Widgets = new SmartReactiveList<IWidget>();
                var itemAddedObservable = Observable
                 .FromEventPattern<NotifyCollectionChangedEventArgs>(Widgets, "CollectionChanged")
                 .Where(change => change.EventArgs.Action == NotifyCollectionChangedAction.Reset || change.EventArgs.Action == NotifyCollectionChangedAction.Remove)
                 .Select(change => change.EventArgs)
                 .Where(change => change != null)
                 .Subscribe(change =>
                 {
                     if (change.OldItems == null)
                     {
                         var panels = new List<LayoutPanel>();

                         VisualTreeWalker<LayoutPanel>.Execute(View, panel =>
                         {
                             panels.Add(panel);
                         });

                         foreach (var panel in panels)
                         {
                             View.WidgetDockManager.DockController.Close(panel);
                         }

                     }
                     else
                     {
                         foreach (var item in change.OldItems)
                         {
                             VisualTreeWalker<LayoutPanel>.Execute(View, panel =>
                             {
                                 if (panel.DataContext != item) return;
                                 View.WidgetDockManager.DockController.Close(panel);
                             });

                         };
                     }

                 });

            }

            var notIsLoading = this.WhenAny(z => z.IsLoading, z => !IsLoading);

            ClearCurrentLayout = ReactiveCommand.Create(() => Widgets.Clear(), this.WhenAny(z => z.IsLoading, (x) => !IsLoading));

            SaveLayoutAsFile = ReactiveCommand.Create(() =>
           {
               var layout = GetCurrentLayout();

               var dialog = new SaveFileDialog()
               {
                   FileName = String.Format("LAYOUT_FOR_{0}", State.Name.Replace(" ", "_")),
                   DefaultExt = layoutDefaultExt,
                   Filter = layoutDefaultFilter
               };

               if (dialog.ShowDialog() == true)
               {
                   var formatter = new BinaryFormatter();
                   var stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                   formatter.Serialize(stream, layout);
                   stream.Close();
               }

           });

            SaveLayout = ReactiveCommand.CreateFromTask(() => SaveCurrentLayoutAsDefault(), notIsLoading);

            LoadLayoutFromFile = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFileDialog()
                {
                    Multiselect = false,
                    Filter = layoutDefaultFilter
                };

                if (dialog.ShowDialog() == true)
                {
                    var stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var formatter = new BinaryFormatter();
                    var layout = (WorkspaceLayout)formatter.Deserialize(stream);
                    await LoadLayout(layout);
                }

            }, notIsLoading);

            SaveTaggedLayout = ReactiveCommand.Create(SaveCurrentLayoutAsTagged, this.WhenAny(vm => vm.IsLoading, vm => vm.TaggedLayoutLabel, (isloading, taggedLayoutName) => !isloading.Value && !String.IsNullOrEmpty(taggedLayoutName.Value)));
            DeleteTaggedLayout = ReactiveCommand.Create<WorkspaceLayout>((workspaceLayout) =>
            {

            }, notIsLoading);

            LoadTaggedLayout = ReactiveCommand.Create<WorkspaceLayout>(async (workspaceLayout) =>
           {
             await  LoadLayout(workspaceLayout);

           }, notIsLoading);

            SaveTemplateLayout = ReactiveCommand.CreateFromTask(SaveCurrentLayoutAsTemplate, this.WhenAny(z => z.IsLoading, (x) => !IsLoading));

            OpenNewWorkspace = ReactiveCommand.Create(() =>
            {
                Dashboard.CreateWorkspace(State, false);
            });

            if (loadLayout)
            {
                this.ExecuteOnCurrentDispatcher(async () =>
                {
                    var layout = await FindRelevantLayout();

                    if (null == layout) return;

                    await LoadLayout(layout);

                });

            }
        }

        #region Layout Management

        private IEnumerable<LayoutPanel> FindWidgetPanels(BaseLayoutItem item)
        {
            var panels = new List<LayoutPanel>();

            if (item is LayoutPanel && (item as LayoutPanel).DataContext is IWidget) panels.Add(item as LayoutPanel);
            if (item is LayoutPanel && (item as LayoutPanel).Content != null && (item as LayoutPanel).Content is BaseLayoutItem) panels.AddRange(FindWidgetPanels((item as LayoutPanel).Content as BaseLayoutItem));
            if (item is LayoutGroup && (item as LayoutGroup).HasVisibleItems)
            {

                foreach (var panel in (item as LayoutGroup).Items.Cast<BaseLayoutItem>())
                {
                    panels.AddRange(FindWidgetPanels(panel));
                }
            }

            return panels;
        }

        private IEnumerable<LayoutPanel> FindEmptyPanels(BaseLayoutItem item)
        {
            var panels = new List<LayoutPanel>();

            if (item is LayoutPanel && (item as LayoutPanel).Content == null) panels.Add(item as LayoutPanel);
            if (item is LayoutPanel && (item as LayoutPanel).Content != null && (item as LayoutPanel).Content is BaseLayoutItem) panels.AddRange(FindEmptyPanels((item as LayoutPanel).Content as BaseLayoutItem));
            if (item is LayoutGroup && (item as LayoutGroup).HasVisibleItems)
            {

                foreach (var panel in (item as LayoutGroup).Items.Cast<BaseLayoutItem>())
                {
                    panels.AddRange(FindEmptyPanels(panel));
                }
            }

            return panels;
        }

        private async Task<WorkspaceLayout> FindRelevantLayout()
        {
            var layout = await CacheStrategy.PersistantCache.Get(String.Format("{0}{1}", DxDashboardConstants.layoutWidgetsKey, State.Name)); 

            if (null != layout) return layout;

            layout = await CacheStrategy.PersistantCache.Get(String.Format("{0}{1}", DxDashboardConstants.layoutWidgetsTemplateKey, State.Tag));

            return layout;
        }

        private void SaveFloatingWidgetsGridLayouts()
        {
            var panels = new List<LayoutPanel>();

            foreach (var floatGroup in View.WidgetDockManager.FloatGroups.Cast<FloatGroup>())
            {
                panels.AddRange(floatGroup.Items.Select(x => FindWidgetPanels(x))
                    .Aggregate((x, y) => x.Concat(y)));
            }

            panels.ForEach(panel => SetNameOnPanel(panel));

            foreach (var panel in panels)
            {
                var widget = (panel.Content as IWidget);

                if (widget == null) continue;

                VisualTreeWalker<LayoutPanel>.Execute(panel, layoutPanel =>
                {
                    if (layoutPanel.Name != widget.ParentName) return;

                    var layoutId = DateTime.Now.Ticks;

                    if (widget.AllowGridSaveLayout)
                    {
                        VisualTreeWalker<GridControl>.Execute(widget.View, grid =>
                        {
                            using (var ms = new MemoryStream())
                            {
                                grid.SaveLayoutToStream(ms);

                                ms.Position = 0;

                                widget.GridsLayout.RemoveAll(y => y.GridLayoutId != layoutId);

                                widget.GridsLayout.Add(new GridLayoutItem()
                                {
                                    GridLayoutId = layoutId,
                                    Layout = ms.ToArray(),
                                    WidgetId = widget.DockId
                                }
                                );
                            }
                        });

                        VisualTreeWalker<PivotGridControl>.Execute(widget.View, grid =>
                        {
                            using (var ms = new MemoryStream())
                            {
                                grid.SaveLayoutToStream(ms);
                                ms.Position = 0;

                                widget.PivotGridsLayout.RemoveAll(y => y.GridLayoutId != layoutId);

                                widget.PivotGridsLayout.Add(new GridLayoutItem()
                                {
                                    GridLayoutId = layoutId,
                                    Layout = ms.ToArray(),
                                    WidgetId = widget.DockId
                                }
                                );
                            }
                        });
                    }
                });
            }
        }

        private void RestoreFloatingWidgetsGridLayouts(IEnumerable < WidgetDescriptor > descriptors)
        {
            if (null == View) return;

            var panels = new List<LayoutPanel>();

            foreach (var floatGroup in View.WidgetDockManager.FloatGroups.Cast<FloatGroup>())
            {
                panels.AddRange(floatGroup.Items.Select(x => FindWidgetPanels(x)).Aggregate((x, y) => x.Concat(y)));
            }

            if (panels.Count() == 0) return;

            foreach (var panel in panels)
            {
                var widget = panel.DataContext as IWidget;

                var descriptor = descriptors.FirstOrDefault(des => des.ParentName == widget.ParentName);

                RestoreGridLayoutInternal(panel.Content as UserControl, descriptor);

            }

        }

        private void RestoreFloatingWidgets()
        {
            var panels = new List<LayoutPanel>();

            foreach (var floatGroup in View.WidgetDockManager.FloatGroups.Cast<FloatGroup>())
            {
                panels.AddRange(floatGroup.Items.Select(x => FindEmptyPanels(x)).Aggregate((x, y) => x.Concat(y)));
            }

            if (panels.Count() == 0) return;

            foreach(var panel in panels)
            {
                var canditate = Widgets.FirstOrDefault(widget => widget.ParentName == panel.Name);

                VisualTreeWalker<LayoutPanel>.Execute(panel, FillPanel, Widgets.Select(w=> w));
            }
        }

        private void RestoreWorkspaceLayout(WorkspaceLayout layout)
        {
            if (null == View) return;

            View.WidgetDockManager.RestoreLayoutFromStream(new MemoryStream(layout.DockingLayout));

            var instances = new List<IWidget>();

            foreach (var widget in layout.Widgets)
            {
                var widgetInstance = Dashboard.CreateWidget(Type.GetType(widget.Type), widget.ViewModelId).Result;
                if (null == widgetInstance) throw new MissingWidgetException(widget.Type);
                widgetInstance.ParentName = widget.ParentName;
                widgetInstance.GridsLayout = widget.GridsLayout;
                widgetInstance.PivotGridsLayout = widget.PivotGridsLayout;
                widgetInstance.DockId = widget.DockId;
                instances.Add(widgetInstance);
            }

            Widgets.AddRange(instances, false);

            VisualTreeWalker<LayoutPanel>.Execute(View, FillPanel, instances);

            RestoreFloatingWidgets();

        }


        private void RestoreGridLayoutInternal(UserControl control, WidgetDescriptor descriptor)
        {

             this.ExecuteOnCurrentDispatcher(() =>
             {

                 var i = -1;

                 VisualTreeWalker<GridControl>.Execute(control, grid =>
                {
                    if (descriptor.GridsLayout.Count > ++i)
                    {
                        grid.RestoreLayoutFromStream(new MemoryStream(descriptor.GridsLayout[i].Layout));
                    }

                });

                 i = -1;

                 VisualTreeWalker<PivotGridControl>.Execute(control, grid =>
                 {
                     if (descriptor.PivotGridsLayout.Count > ++i)
                     {
                         grid.RestoreLayoutFromStream(new MemoryStream(descriptor.PivotGridsLayout[i].Layout));
                     }
                 });

             });

        }

        private void RestoreGridLayouts(IEnumerable<WidgetDescriptor> descriptors)
        {
            var processedPanels = new List<LayoutPanel>();

            foreach (var widget in Widgets)
            {

                VisualTreeWalker<LayoutPanel>.Execute(View, layoutPanel =>
                 {
                     if (layoutPanel.Name != widget.ParentName) return;

                     if (processedPanels.Contains(layoutPanel)) return;

                     processedPanels.Add(layoutPanel);

                     var descriptor = descriptors.FirstOrDefault(des => des.ParentName == widget.ParentName);

                      RestoreGridLayoutInternal(layoutPanel.Content as UserControl, descriptor);

                 });
                
            }

            RestoreFloatingWidgetsGridLayouts(descriptors);
        }

        public async Task LoadLayout(WorkspaceLayout providedLayout = null)
        {
             Widgets.Clear();

            var layout = providedLayout ??  await FindRelevantLayout();

            if (null == layout) return;

             RestoreWorkspaceLayout(layout);

             RestoreGridLayouts(layout.Widgets);
        }

        private void FillPanel(LayoutPanel panel, IEnumerable<IWidget> widgets)
        {
            var list = widgets.ToList();

            var widget = list.FirstOrDefault(x => x.ParentName == panel.Name);
            var position = list.IndexOf(widget);

            if (null== widget) return;

            panel.DataContext = widget;

            panel.SetBinding(ContentItem.ContentProperty, "View");
            panel.SetBinding(BaseLayoutItem.CaptionProperty, "Header");

        }

        private void SetNameOnPanel(LayoutPanel panel)
        {
            if (panel.DataContext is IWidget content)
            {
                panel.Name = panel.BindableName = content.ParentName;
            }
        }

        #endregion


    }
}
