using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        public ObservableCollection<Driver> DriversSource { get; set; }
        private string _driverName;
        bool freezDrivers = false;

        Driver _driver;
        public Driver Driver 
        {
            get => _driver;
            set 
            {
                _driver = value;
                OnPropertyChanged(nameof(Driver));
            }
        }

        public string DriverName 
        {
            get => _driverName;
            set 
            {
                _driverName = value;
                OnPropertyChanged(nameof(DriverName));
            }
        }

        private void setDriver()
        {
            if (_driver == null && !string.IsNullOrEmpty(_driverName))
            {
                freezDrivers = true;
                Driver driver = _context.Drivers
                        .FirstOrDefault(s => s.Name.ToLower() == DriverName.ToLower());

                if (driver is null)
                    Driver = new Driver { Name = CustomerName };
                else
                {
                    Driver = driver;
                }
                freezDrivers = false;
            }
        }
    }
}
