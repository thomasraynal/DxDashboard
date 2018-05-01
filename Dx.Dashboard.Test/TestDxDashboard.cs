using Dx.Dashboard.Common;
using Dx.Dashboard.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dx.Dashboard.Test
{

    [TestFixture]
    public class TestDxDashboard
    {
        private IDashboard<TestWorkspaceState> _testDashboard;
        private IWorkspace<TestWorkspaceState> _testWorkspace;
        private WidgetAttribute _testWidget;
        private WorkspaceLayout _currentLayout;


        private Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }


        [Test, Order(1)]
        public void ShouldCreateDashboard()
        {
            _testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();

            Assert.AreEqual(2, _testDashboard.AvailableWidgets.Count());

            Assert.AreEqual(0, _testDashboard.AvailableWorkspaces.Count());
        }

        [Test, Order(2)]
        public async Task ShouldCreateWorspace()
        {
            await _testDashboard.CreateNewWorkspace(new TestWorkspaceState("Yadayada", "TYPE"), true);

            Assert.AreEqual(1, _testDashboard.AvailableWorkspaces.Count());

            await _testDashboard.CreateNewWorkspace(new TestWorkspaceState("Yadayada", "TYPE"), true);

            Assert.AreEqual(1, _testDashboard.AvailableWorkspaces.Count());

            _testWorkspace = _testDashboard.AvailableWorkspaces.First();

            Assert.AreEqual("Yadayada", _testDashboard.CurrentWorkspace.State.Name);
        }


        [Test, Order(3)]
        public async Task ShouldCreateWidget()
        {
            _testWidget = _testDashboard.AvailableWidgets.First();

            await StartSTATask(async () =>
            {
                await _testDashboard.CreateWidget.ExecuteIfPossible(_testWidget.Name);
            });

            Assert.AreEqual(1, _testWorkspace.Widgets.Count());

            Assert.AreEqual(_testWidget.Name, _testWorkspace.Widgets.First().Name);
        }


        [Test, Order(4)]
        public async Task ShouldSaveLayout()
        {
            //to do - default
            //to do - templated
            //to do - tagged

            await StartSTATask(() =>
           {
               _testWorkspace.View = new Workspace();
               return Task.CompletedTask;
           });

            _currentLayout = _testWorkspace.GetCurrentLayout();

            await _testWorkspace.SaveLayout.ExecuteIfPossible();
            await _testWorkspace.SaveTemplateLayout.ExecuteIfPossible();

            await _testDashboard.CreateNewWorkspace(new TestWorkspaceState("Yadayada2", "TYPE"), true);

            //  await Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => Trace.WriteLine("do")), DispatcherPriority.Normal);

            Assert.AreEqual(2, _testDashboard.AvailableWorkspaces.Count());

            var newWorkspace = _testDashboard.AvailableWorkspaces[1];

            //to do - excecute dispatcher queue

            //Assert.AreEqual(_currentLayout.Widgets.Count(), newWorkspace.Widgets.Count());

            //Assert.AreEqual(_testWidget.Name, newWorkspace.Widgets.First().Name);
        }

        [Test, Order(4)]
        public Task ShouldSaveWidgetProperties()
        {
            return Task.CompletedTask;
        }

        [Test, Order(5)]
        public void ShouldCreateWidgetMenu()
        {
           var menu = _testDashboard.CreateWidgetMenu("Widgets", DevExpressHelper.GetGlyph("GlobalColorScheme_16x16.png"));

            Assert.AreEqual(1, menu.Items.Count());

            var category = menu.Items.First() as ListItemDescriptor;

            Assert.AreEqual(category.Caption, TestConstants.TestCategory.Path);
            Assert.AreEqual(2, category.Items.Count());
        }



     
    }
}
