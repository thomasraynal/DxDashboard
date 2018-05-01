using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Common.Infra
{
    public static class ICanLogEx
    {
        private static IClientLogger _logger;

        static ICanLogEx()
        {
            _logger = AppCore.Instance.Get<IClientLogger>();
        }

        public static IClientLogger Logger
        {
            get { return _logger; }
        }

        public static void Fatal(this ICanLog @this, string message)
        {
            Logger.Fatal(message);
        }

        public static void Error(this ICanLog @this, string message)
        {
            Logger.Error(message);
        }

        public static void Warn(this ICanLog @this, string message)
        {
            Logger.Warn(message);
        }

        public static void Info(this ICanLog @this, string message)
        {
            Logger.Info(message);
        }

        public static void Fatal(this ICanLog @this, Exception ex)
        {
            Logger.Fatal(ex);
        }

        public static void Error(this ICanLog @this, Exception ex)
        {
            Logger.Error(ex);
        }

        public static void Warn(this ICanLog @this, Exception ex)
        {
            Logger.Warn(ex);
        }

        public static void Info(this ICanLog @this, Exception ex)
        {
            Logger.Info(ex);
        }

        public static void Fatal(this ICanLog @this, string message, Exception ex)
        {
            Logger.Fatal(message, ex);
        }

        public static void Error(this ICanLog @this, string message, Exception ex)
        {
            Logger.Error(message, ex);
        }

        public static void Warn(this ICanLog @this, string message, Exception ex)
        {
            Logger.Warn(message, ex);
        }

        public static void Info(this ICanLog @this, string message, Exception ex)
        {
            Logger.Info(message, ex);
        }

    }

    public interface ICanLog
    {
    }
}
