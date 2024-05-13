using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
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
                if (_driver != null)
                {
                    setTransportCompanyByDriver();
                    setCarByDriver();
                    setTraillerByDriver();
                }
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

        private void setTransportCompanyByDriver()
        {
            TransportCompany comp = _context.TransportCompanies.AsNoTracking().FirstOrDefault(s => s.Drivers.Any(driver => driver.DriverId == Driver.DriverId));
            if (comp != null)
            {
                CompaniesSource.Add(comp);
                TransportCompany = comp;
            }
        }

        private void setCarByDriver()
        {
            CarSource = _context.Cars.AsNoTracking().Include(car => car.CarBrand)
                                                    .Select(s => s)
                                                    .Where(s => s.Drivers.Any(driver => driver.DriverId == Driver.DriverId))
                                                    .ToList();
            if (CarSource != null)
            {
                Car = CarSource.FirstOrDefault();
            }
        }

        private void setTraillerByDriver()
        {
            TraillerSource = _context.Traillers.AsNoTracking().Include(trailler => trailler.TraillerBrand)
                                                                 .Select(t => t)
                                                                 .Where(t => t.Drivers.Any(driver => driver.DriverId == Driver.DriverId))
                                                                 .ToList();
            if (TraillerSource != null)
            {
                Trailler = TraillerSource.FirstOrDefault();
            }
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
        public TransportCompany TransportCompany
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged(nameof(TransportCompany));
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

        //------------------------ CarBrand --------------------------------
        public DelegateCommand GetCarBrands { get; private set; }
        private List<CarBrand> _carBrandSource;
        public List<CarBrand> CarBrandSource
        {
            get => _carBrandSource;
            set
            {
                _carBrandSource = value;
                OnPropertyChanged(nameof(CarBrandSource));
            }
        }

        private CarBrand _carBrand;
        public CarBrand CarBrand
        {
            get => _carBrand;
            set
            {
                _carBrand = value;
                OnPropertyChanged(nameof(CarBrand));
            }
        }

        private void getCarBrands(object e)
        {
            string text = e as string;
            CarBrandSource = _context.CarBrands
                            .AsNoTracking()
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(c => c.Name)
                            .Take(5)
                            .ToList();
        }

        //------------------------ TraillerBrand --------------------------------
        public DelegateCommand GetTraillerBrands { get; private set; }
        private List<TraillerBrand> _traillerBrandSource;
        public List<TraillerBrand> TraillerBrandSource
        {
            get => _traillerBrandSource;
            set
            {
                _traillerBrandSource = value;
                OnPropertyChanged(nameof(TraillerBrandSource));
            }
        }

        private TraillerBrand _traillerBrand;
        public TraillerBrand TraillerBrand 
        {
            get => _traillerBrand;
            set 
            {
                _traillerBrand = value;
                OnPropertyChanged(nameof(TraillerBrand));
            }
        }

        private void getTraillerBrands(object e) 
        {
            string text = e as string;
            TraillerBrandSource = _context.TraillerBrands
                                  .AsNoTracking()
                                  .Include(tb => tb.Name.ToLower().Contains(text.ToLower()))
                                  .OrderBy(tb => tb.Name)
                                  .Take(5)
                                  .ToList();
        }

        //------------------------ Car --------------------------------
        private List<Car> _carSource;
        public List<Car> CarSource 
        {
            get => _carSource;
            set 
            {
                _carSource = value;
                OnPropertyChanged(nameof(CarSource));
            }
        }

        private Car _car;
        public Car Car 
        {
            get => _car;
            set 
            {
                _car = value;
                if (!CarBrandSource.Contains(_car.CarBrand)) CarBrandSource.Add(_car.CarBrand);
                CarBrand = _car.CarBrand;
                OnPropertyChanged(nameof(Car));
            }
        }


        //------------------------ Trailler --------------------------------
        private List<Trailler> _traillerSource;
        public List<Trailler> TraillerSource 
        {
            get => _traillerSource;
            set 
            {
                _traillerSource = value;
                OnPropertyChanged(nameof(TraillerSource));
            }
        }
        private Trailler _trailler;
        public Trailler Trailler 
        {
            get => _trailler;
            set 
            {
                _trailler = value;
                if (!TraillerBrandSource.Contains(_trailler.TraillerBrand)) TraillerBrandSource.Add(_trailler.TraillerBrand);
                TraillerBrand = _trailler.TraillerBrand;
            }
        }

        private void getTraillers(object e)
        {
            string text = e as string;
            TraillerSource = _context.Traillers
                            .AsNoTracking()
                            .Include(trailler => trailler.TraillerBrand)
                            .Where(t => t.TraillerBrand.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(t => TraillerBrand.Name)
                            .Take(5)
                            .ToList();
        }
    }
}
