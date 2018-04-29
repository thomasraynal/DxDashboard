using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dx.Dashboard.Core
{
    public class CustomItemDescriptor : BaseItemDescriptor
    {
        public CustomItemDescriptor(string caption, UserControl control) : base(caption, null)
        {
            Content = control;
        }

        private UserControl _content;
        public UserControl Content
        {
            get { return _content; }
            set { this.RaiseAndSetIfChanged(ref _content, value); }
        }
       
    }
}
