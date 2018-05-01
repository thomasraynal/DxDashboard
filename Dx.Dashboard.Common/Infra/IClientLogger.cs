using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Common
{
    public interface IClientLogger
    {
        void Fatal(string message);
        void Error(string message);
        void Warn(string message);
        void Info(string message);
        void Fatal(Exception ex);
        void Error(Exception ex);
        void Warn(Exception ex);
        void Info(Exception ex);
        void Fatal(string message, Exception ex);
        void Error(string message, Exception ex);
        void Warn(string message, Exception ex);
        void Info(string message, Exception ex);
    }
}
