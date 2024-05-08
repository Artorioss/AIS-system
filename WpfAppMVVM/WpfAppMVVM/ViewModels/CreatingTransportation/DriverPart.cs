using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        private List<Driver> _driversSource;
        public List<Driver> DriversSource 
        {
            get => _driversSource;
            set 
            {
                _driversSource = value;
                OnPropertyChanged(nameof(DriversSource));
            }
        }
        public DelegateCommand SetDriver { get; private set; } 
        private string _driverName;
        bool freezDrivers = false;

        Driver _driver;
        public Driver Driver 
        {
            get => _driver;
            set 
            {
                _driver = value;
                if (freezDrivers)
                {
                    var driver = DriversSource.SingleOrDefault(x => x.DriverId == value.DriverId);
                    if (driver != null)
                    {
                        DriversSource.Remove(driver);
                        DriversSource.Add(value);
                    }
                    else
                    {
                        DriversSource.Clear();
                        DriversSource.Add(value);
                    }
                }
                OnPropertyChanged(nameof(Driver));
            }
        }

        public string DriverName 
        {
            get => _driverName;
            set 
            {
                _driverName = value;
                if (!freezDrivers) getDrivers();
                OnPropertyChanged(nameof(DriverName));
            }
        }

        private bool _isDropDownOpenDrivers;
        public bool IsDropDownOpenDrivers
        {
            get => _isDropDownOpenDrivers;
            set
            {
                _isDropDownOpenDrivers = value;
                OnPropertyChanged(nameof(IsDropDownOpenDrivers));
            }
        }

        private void setDriver()
        {
            if (_driver == null && !string.IsNullOrEmpty(_driverName))
            {
                freezDrivers = true;
                Driver driver = _context.Drivers
                        .FirstOrDefault(s => s.Name.ToLower().Contains(DriverName.ToLower()));

                if (driver is null)
                    Driver = new Driver { Name = CustomerName };
                else
                {
                    Driver = driver;
                }
                freezDrivers = false;
            }
        }

        private void getDrivers()
        {
            if (string.IsNullOrEmpty(DriverName))
            {
                IsDropDownOpenDrivers = false;
                Driver = null;
            }

            if (Driver == null)
            {
                DriversSource = _context.Drivers.AsNoTracking()
                                .Where(c => c.Name.ToLower().Contains(DriverName.ToLower()))
                                .OrderBy(c => c.Name)
                                .Take(5)
                                .ToList();

                OnPropertyChanged(nameof(DriversSource));
            }
            IsDropDownOpenDrivers = DriversSource.Count > 0;
        }


        //------------------------ CompanyTransportation --------------------------------
        private List<TransportCompany> _companiesSource;
        public List<TransportCompany> CompaniesSource 
        {
            get => _companiesSource;
            set 
            {
                _companiesSource = value;
                OnPropertyChanged(nameof(CompaniesSource));
            }
        }
        public DelegateCommand SetCompany { get; private set; }
        bool freezCompanies = false;
        TransportCompany _company;
        public TransportCompany Company 
        {
            get => _company;
            set 
            {
                _company = value;
                if (freezCompanies)
                {
                    var comp = CompaniesSource.SingleOrDefault(x => x.TransportCompanyId == value.TransportCompanyId);
                    if (comp != null)
                    {
                        CompaniesSource.Remove(comp);
                        CompaniesSource.Add(value);
                    }
                    else
                    {
                        CompaniesSource.Clear();
                        CompaniesSource.Add(value);
                    }
                }
                OnPropertyChanged(nameof(Company));
            }
        }

        private string _companyName;
        public string CompanyName 
        {
            get => _companyName;
            set 
            {
                _companyName = value;
                if (!freezDrivers) getCompanies();
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private bool _isDropDownOpenCompanies;
        public bool IsDropDownOpenCompanies
        {
            get => _isDropDownOpenCompanies;
            set
            {
                _isDropDownOpenCompanies = value;
                OnPropertyChanged(nameof(IsDropDownOpenCompanies));
            }
        }

        private void setCompany()
        {
            if (_company == null && !string.IsNullOrEmpty(_companyName))
            {
                freezCompanies = true;
                TransportCompany comp = _context.TransportCompanies
                        .FirstOrDefault(s => s.Name.ToLower().Contains(CompanyName.ToLower()));

                if (comp is null)
                    Company = new TransportCompany { Name = CompanyName };
                else
                {
                    Company = comp;
                }
                freezDrivers = false;
            }
        }

        private void getCompanies()
        {
            if (string.IsNullOrEmpty(CompanyName))
            {
                IsDropDownOpenCompanies = false;
                Company = null;
            }

            if (Company == null)
            {
                CompaniesSource = _context.TransportCompanies.AsNoTracking()
                                .Where(c => c.Name.ToLower().Contains(CompanyName.ToLower()))
                                .OrderBy(c => c.Name)
                                .Take(5)
                                .ToList();

                OnPropertyChanged(nameof(CompaniesSource));
            }
            IsDropDownOpenCompanies = CompaniesSource.Count > 0;
        }
    }
}
