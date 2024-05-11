using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public DelegateCommand GetDrivers { get; private set; } 

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

        private void getDrivers(object e)
        {
            string text = e as string;
            DriversSource = _context.Drivers.AsNoTracking()
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(c => c.Name)
                            .Take(5)
                            .ToList();   
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
        public DelegateCommand GetCompanies { get; private set; }

        TransportCompany _company;
        public TransportCompany Company 
        {
            get => _company;
            set 
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        private void getCompanies(object e)
        {
            string text = e as string;
            CompaniesSource = _context.TransportCompanies.AsNoTracking()
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(c => c.Name)
                            .Take(5)
                            .ToList();
        }
    }
}
