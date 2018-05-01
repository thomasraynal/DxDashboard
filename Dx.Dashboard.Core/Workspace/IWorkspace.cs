﻿using DevExpress.Xpf.Docking;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dx.Dashboard.Core
{
    public interface IWidgetHolder
    {
        bool IsSelected { get; }
        Guid Id { get; }
        Workspace View { get; set; }
        SmartReactiveList<IWidget> Widgets { get; }
    }

    public interface IWorkspace<TState>: IWidgetHolder
        where TState : class, IWorkspaceState
    {
        Task Initialize(TState state);
        TState State { get; }
    }
}