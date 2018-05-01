using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public static class StringExtensions
    {
        public static string IgnoreCaseAndSpaces(this string str)
        {
            return str.ToLower().Replace(" ", "");
        }
    }
}
