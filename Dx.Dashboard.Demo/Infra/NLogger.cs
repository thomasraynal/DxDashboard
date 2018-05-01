using Dx.Dashboard.Common;
using Dx.Dashboard.Common.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Demo
{
    public class NLogger : IClientLogger
    {
        private Logger _logger;

        public NLogger(IDashboardConfiguration configuration)
        {
            var logPath = configuration.KeyValueStore["logs"];

            var logConfig = new LoggingConfiguration();

            var target = new FileTarget
            {
                FileName = Path.Combine(logPath, "log.txt"),
                Layout = @"${time} [${uppercase:${level}}] ${message} ${exception:format=ToString,StackTrace}",
                ArchiveAboveSize = 1024 * 1024 * 2,
                ArchiveNumbering = ArchiveNumberingMode.Sequence
            };

            logConfig.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Info, target));
            LogManager.Configuration = logConfig;

            _logger = LogManager.GetLogger(configuration.ApplicationName);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception ex)
        {
            _logger.Error(ex);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception ex)
        {
            _logger.Fatal(ex);
        }

        public void Fatal(string message, Exception ex)
        {
            _logger.Fatal(ex, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(Exception ex)
        {
            _logger.Info(ex);
        }

        public void Info(string message, Exception ex)
        {
            _logger.Info(ex, message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(Exception ex)
        {
            _logger.Warn(ex);
        }

        public void Warn(string message, Exception ex)
        {
            _logger.Warn(ex, message);
        }
    }
}
