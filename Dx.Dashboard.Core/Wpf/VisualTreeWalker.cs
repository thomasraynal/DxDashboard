using DevExpress.Xpf.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Reactive;

namespace Dx.Dashboard.Core
{

    public static class VisualTreeWalker<TTargetControl> where TTargetControl : DependencyObject
    {
        public static void Execute(Visual root, Action<TTargetControl> action)
        {
            ExecuteInternal(root, new Action<TTargetControl, Unit>((tgt, unit) => action(tgt)), Unit.Default);
        }

        public static void Execute<TParam>(Visual root, Action<TTargetControl, TParam> action, TParam param)
        {
            ExecuteInternal(root, action, param);
        }

        private static void ExecuteInternal<TParam>(Visual control, Action<TTargetControl, TParam> action, TParam param)
        {

            if (null == control) return;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(control) - 1; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(control, i);

                var dp = control as DependencyObject;

                if ((dp as TTargetControl != null))
                {
                    action(dp as TTargetControl, param);
                }

                if ((dp as AutoHideGroup != null))
                {
                    var lp = dp as AutoHideGroup;

                    foreach (var item in lp.Items)
                    {
                        var elt = item as TTargetControl;

                        if (null != elt)
                            action(item as TTargetControl, param);
                    }
                }

                else if ((dp as FloatGroup != null))
                {
                    var lp = dp as FloatGroup;

                    foreach (var item in lp.Items)
                    {
                        var elt = item as TTargetControl;

                        if (null != elt)
                            action(item as TTargetControl, param);
                    }
                }

                else if ((dp as TabbedGroup != null))
                {
                    var lp = dp as TabbedGroup;

                    foreach (var item in lp.Items)
                    {
                        var elt = item as TTargetControl;

                        if (null != elt)
                            action(item as TTargetControl, param);

                    }
                }

                if (VisualTreeHelper.GetChildrenCount(v) > 0)
                {
                    ExecuteInternal(v, action, param);
                }
            }
        }
    }


}
