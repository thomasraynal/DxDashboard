using Dx.Dashboard.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Tests
{
    public class TraceLogger : IClientLogger
    {
        public void Error(string message)
        {
            Trace.WriteLine(message);
        }

        public void Error(Exception ex)
        {
            Trace.WriteLine(ex);
        }

        public void Error(string message, Exception ex)
        {
           Trace.WriteLine(message);
        }

        public void Fatal(string message)
        {
           Trace.WriteLine(message);
        }

        public void Fatal(Exception ex)
        {
           Trace.WriteLine(ex);
        }

        public void Fatal(string message, Exception ex)
        {
           Trace.WriteLine(message);
        }

        public void Info(string message)
        {
           Trace.WriteLine(message);
        }

        public void Info(Exception ex)
        {
           Trace.WriteLine(ex);
        }

        public void Info(string message, Exception ex)
        {
           Trace.WriteLine(message);
        }

        public void Warn(string message)
        {
           Trace.WriteLine(message);
        }

        public void Warn(Exception ex)
        {
           Trace.WriteLine(ex);
        }

        public void Warn(string message, Exception ex)
        {
           Trace.WriteLine(message);
        }
    }
}
