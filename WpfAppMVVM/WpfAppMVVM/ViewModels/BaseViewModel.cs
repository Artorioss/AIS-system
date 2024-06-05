using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppMVVM.Model.EfCode;

namespace WpfAppMVVM.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected TransportationEntities _context = (Application.Current as App)._context;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
