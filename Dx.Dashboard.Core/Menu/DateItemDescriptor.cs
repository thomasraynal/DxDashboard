using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx.Dashboard.Core
{
    public class DateItemDescriptor : BaseItemDescriptor
    {
        public DateItemDescriptor(string caption, ReactiveList<DateTime> availableDates, DateTime selectedDate) : base(caption, null)
        {
            AvailableDates = availableDates;
            SelectedDate = selectedDate;
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { this.RaiseAndSetIfChanged(ref _selectedDate, value); }
        }

        private ReactiveList<DateTime> _availableDates;
        public ReactiveList<DateTime> AvailableDates
        {
            get { return _availableDates; }
            set { this.RaiseAndSetIfChanged(ref _availableDates, value); }
        }

    }
}
