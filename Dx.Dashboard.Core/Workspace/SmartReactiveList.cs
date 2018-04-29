using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
   public class SmartReactiveList<TItem> : ObservableCollection<TItem>
    {
        public void AddRange(IEnumerable<TItem> collection, bool raisePropertyChanged)
        {
            foreach(var item in collection)
            {
                this.Items.Add(item);
            }

            if (raisePropertyChanged)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
