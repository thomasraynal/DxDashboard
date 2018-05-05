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
using System.Windows;
using System.Windows.Threading;

namespace Dx.Dashboard.Test
{
    //TO DO - reset DI registry
    [TestFixture]
    public class TestDxDashboard
    {

        private Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();

            var currentDispatcher = AppCore.Instance.Get<Dispatcher>();

            var thread = new Thread(() =>
            {
                try
                {
                    AppCore.Instance.ObjectProvider.Configure((ex) =>
                    {
                        ex.For<Dispatcher>().ClearAll().Use(Dispatcher.CurrentDispatcher);
                    });

                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
                finally
                {
                    AppCore.Instance.ObjectProvider.Configure((ex) =>
                    {
                        ex.For<Dispatcher>().ClearAll().Use(currentDispatcher);
                    });

                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        private void Wait(int count)
        {
            for(var i= 0; i< count; i++)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
            }
        }

        private async Task<IWorkspace<TestWorkspaceState>> CreateWorkspace(IDashboard<TestWorkspaceState> dashboard, TestWorkspaceState state, bool loadLayout)
        {
            await dashboard.CreateWorkspace(state, loadLayout);
            Wait(1);
            dashboard.CurrentWorkspace.View = new Workspace();
            return dashboard.CurrentWorkspace;
        }

        public void ShouldCreateDashboard()
        {
            var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();

            Assert.AreEqual(2, testDashboard.AvailableWidgets.Count());

            Assert.AreEqual(0, testDashboard.AvailableWorkspaces.Count());
        }

        [Test]
        public async Task ShouldCreateWorspace()
        {
            var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();

            await testDashboard.CreateWorkspace(new TestWorkspaceState("Yadayada", "TYPE"), true);

            Assert.AreEqual(1, testDashboard.AvailableWorkspaces.Count());

            await testDashboard.CreateWorkspace(new TestWorkspaceState("Yadayada", "TYPE"), true);

            Assert.AreEqual(1, testDashboard.AvailableWorkspaces.Count());

            var testWorkspace = testDashboard.AvailableWorkspaces.First();

            Assert.AreEqual("Yadayada", testDashboard.CurrentWorkspace.State.Name);

        }

        [Test]
        public async Task ShouldCreateWidget()
        {
            await StartSTATask(async () =>
            {
                var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();

                await testDashboard.CreateWorkspace(new TestWorkspaceState("Yadayada", "TYPE"), true);

                var testWorkspace = testDashboard.AvailableWorkspaces.First();

                var testWidget = testDashboard.AvailableWidgets.First();
                
                await testDashboard.CreateWidgetCommand.ExecuteIfPossible(testWidget.Name);
            
                Assert.AreEqual(1, testWorkspace.Widgets.Count());

                Assert.AreEqual(testWidget.Name, testWorkspace.Widgets.First().Name);

                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();


            });
        }

        [Test]
        public async Task ShouldSaveDefaultLayout()
        {

            await StartSTATask(async () =>
          {
              var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();
              var testWorkspace = await CreateWorkspace(testDashboard, new TestWorkspaceState("Yadayada", "TYPE"), true);
              var testWidget = testDashboard.AvailableWidgets.First();
              await testDashboard.CreateWidgetCommand.ExecuteIfPossible(testWidget.Name);
              var currentLayoutWidgets = testWorkspace.Widgets.Count();

              await testWorkspace.SaveLayout.ExecuteIfPossible();

              testDashboard.AvailableWorkspaces.Clear();

              var newWorkspace = await CreateWorkspace(testDashboard, new TestWorkspaceState("Yadayada", "TYPE"), true);

              Wait(3);

              Assert.AreEqual(currentLayoutWidgets, newWorkspace.Widgets.Count());

              Assert.AreEqual(testWidget.Name, newWorkspace.Widgets.First().Name);

            
          });

        }

        [Test]
        public void ShouldSaveTemplatedLayout()
        {

             StartSTATask(async () =>
            {
                var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();
                var testWorkspace = await CreateWorkspace(testDashboard, new TestWorkspaceState("Yadayada2", "TYPE"), true);

                var testWidget = testDashboard.AvailableWidgets.First();
                await testDashboard.CreateWidgetCommand.ExecuteIfPossible(testWidget.Name);
                var currentLayoutWidgets = testWorkspace.Widgets.Count();

                await testWorkspace.SaveLayout.ExecuteIfPossible();
                await testWorkspace.SaveTemplateLayout.ExecuteIfPossible();

                var newWorkspace = await CreateWorkspace(testDashboard, new TestWorkspaceState("Yadayada3", "TYPE"), true);

                Wait(3);

                Assert.AreEqual(2, testDashboard.AvailableWorkspaces.Count());

                Assert.AreEqual(currentLayoutWidgets, newWorkspace.Widgets.Count());

                Assert.AreEqual(testWidget.Name, newWorkspace.Widgets.First().Name);

              
            }).Wait();

        }

        [Test]
        public async Task ShouldSaveTaggedLayout()
        {

            await StartSTATask(async () =>
            {
                var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();
                var testWorkspace = await CreateWorkspace(testDashboard, new TestWorkspaceState("Yadayada2", "TYPE"), true);
                var testWidget = testDashboard.AvailableWidgets.First();
                await testDashboard.CreateWidgetCommand.ExecuteIfPossible(testWidget.Name);
                var currentLayoutWidgets = testWorkspace.Widgets.Count();

                testWorkspace.TaggedLayoutLabel = "TAGGED";

                await testWorkspace.SaveTaggedLayout.ExecuteIfPossible();

                var newWorkspace = await CreateWorkspace(testDashboard, new TestWorkspaceState("Yadayada3", "TAGGED"), true);

                Wait(3);

                Assert.AreEqual(2, testDashboard.AvailableWorkspaces.Count());

                Assert.AreEqual(currentLayoutWidgets, newWorkspace.Widgets.Count());

                Assert.AreEqual(testWidget.Name, newWorkspace.Widgets.First().Name);

            });
        }

        [Test]
        public void ShouldDiscoverWidgets()
        {
            var testDashboard = AppCore.Instance.Get<IDashboard<TestWorkspaceState>>();

            var menu = testDashboard.MenuItems;

            Assert.AreEqual(1, menu.Count());

            var menuRoot = menu.First() as ListItemDescriptor;

            Assert.AreEqual(menuRoot.Caption, TestConstants.WidgetMenuName);
            Assert.AreEqual(1, menuRoot.Items.Count());

            var menuFirstCategory = menuRoot.Items.First() as ListItemDescriptor;

            Assert.AreEqual(menuFirstCategory.Caption, TestConstants.TestCategory.Path);
            Assert.AreEqual(2, menuFirstCategory.Items.Count());


        }

        [Test]
        public Task ShouldSaveWidgetProperties()
        {
            return Task.CompletedTask;
        }
    }
}
