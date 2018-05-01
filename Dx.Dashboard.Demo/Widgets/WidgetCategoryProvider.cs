using Dx.Dashboard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public static class WidgetCategoryProvider
    {
        private static WidgetCategory _pnlCategory = new WidgetCategory("Pnl", DevExpressHelper.GetGlyph("ExportToXLS_16x16.png"));
        public static WidgetCategory PnLCategory
        {
            get
            {
                return _pnlCategory;
            }
        }

        private static WidgetCategory _strategyChartCategory = new WidgetCategory("Strategies;Charts", DevExpressHelper.GetGlyph("ExportToXLS_16x16.png"));
        public static WidgetCategory StrategyChartCategory
        {
            get
            {
                return _strategyChartCategory;
            }
        }
        private static WidgetCategory _strategyCategory = new WidgetCategory("Strategies", DevExpressHelper.GetGlyph("ExportToXLS_16x16.png"));
        public static WidgetCategory StrategyCategory
        {
            get
            {
                return _strategyCategory;
            }
        }
    }
}
