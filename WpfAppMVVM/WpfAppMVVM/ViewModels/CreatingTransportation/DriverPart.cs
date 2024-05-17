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
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.Entities;
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
                _accountNameBuilder.Driver = value;
                AccountName = _accountNameBuilder.ToString();
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
            DriversSource = _context.Drivers
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(c => c.Name)
                            .Take(5)
                            .ToList();
        }

        private void setTransportCompanyByDriver()
        {
            var val = _context.TransportCompanies.Find(Driver.TransportCompanyId);

            if (val != null)
            {
                CompaniesSource = new List<TransportCompany>();
                CompaniesSource.Add(val);
                TransportCompany = val;
            }
        }

        private void setCarByDriver()
        {
            CarSource = _context.Cars.Include(car => car.Brand)
                                                    .Select(s => s)
                                                    .Where(s => s.Drivers.Any(driver => driver.DriverId == Driver.DriverId))
                                                    .ToList();


            if (CarSource != null && CarSource.Count > 0)
            {
                CarBrandSource = CarSource.Select(car => car.Brand).Distinct(new BrandComparer()).ToList();
                Car = CarSource.FirstOrDefault();
            }
        }

        private void setTraillerByDriver()
        {
            TraillerSource = _context.Traillers
                                               .Include(trailler => trailler.Brand)
                                               .Where(t => t.Drivers.Any(driver => driver.DriverId == Driver.DriverId))
                                               .ToList();

            if (TraillerSource != null && TraillerSource.Count > 0)
            {
                TraillerBrandSource = TraillerSource.Select(trailler => trailler.Brand).Distinct(new BrandComparer()).ToList();
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
            CompaniesSource = _context.TransportCompanies
                                      .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                      .OrderBy(c => c.Name)
                                      .Take(5)
                                      .ToList();
        }

        //private void setDriversByCompany() 
        //{
        //    List<Driver> list = _context.Drivers.AsNoTracking()
        //                                    .Where(driver => driver.TransportCompanyId == TransportCompany.TransportCompanyId)
        //                                    .OrderBy(d => d.Name)
        //                                    .Take(5)
        //                                    .ToList();

        //    if (Driver != null) 
        //    {
        //        var val = list.SingleOrDefault(d => d.DriverId == Driver.DriverId);
        //        if (val == null) list.Add(Driver);
        //        else val = Driver;
        //    }
                
        //    DriversSource = list;
        //}



        //------------------------ CarBrand --------------------------------
        public DelegateCommand GetCarBrands { get; private set; }
        private List<Brand> _carBrandSource;
        public List<Brand> CarBrandSource
        {
            get => _carBrandSource;
            set
            {
                _carBrandSource = value;
                OnPropertyChanged(nameof(CarBrandSource));
            }
        }

        private Brand _carBrand;
        public Brand CarBrand
        {
            get => _carBrand;
            set
            {
                _carBrand = value;
                _accountNameBuilder.CarBrand = value;
                AccountName = _accountNameBuilder.ToString();
                OnPropertyChanged(nameof(CarBrand));
            }
        }

        private void getCarBrands(object e)
        {
            string text = e as string;
            CarBrandSource = _context.Brands
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()) || c.RussianBrandName.ToLower().Contains(text.ToLower()))
                            .OrderBy(c => c.Name)
                            .Take(5)
                            .ToList();
        }

        //------------------------ TraillerBrand --------------------------------
        public DelegateCommand GetTraillerBrands { get; private set; }
        private List<Brand> _traillerBrandSource;
        public List<Brand> TraillerBrandSource
        {
            get => _traillerBrandSource;
            set
            {
                _traillerBrandSource = value;
                OnPropertyChanged(nameof(TraillerBrandSource));
            }
        }

        private Brand _traillerBrand;
        public Brand TraillerBrand 
        {
            get => _traillerBrand;
            set 
            {
                _traillerBrand = value;
                _accountNameBuilder.TraillerBrand = value;
                AccountName = _accountNameBuilder.ToString();
                OnPropertyChanged(nameof(TraillerBrand));
            }
        }

        private void getTraillerBrands(object e) 
        {
            string text = e as string;
            TraillerBrandSource = _context.Brands
                                  .Where(tb => (tb.Name.ToLower().Contains(text.ToLower()) || tb.RussianBrandName.ToLower().Contains(text.ToLower())))
                                  .OrderBy(tb => tb.Name)
                                  .Take(5)
                                  .ToList();
        }

        //------------------------ Car --------------------------------
        public DelegateCommand GetCars { get; private set; }

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
                _accountNameBuilder.Car = value;
                AccountName = _accountNameBuilder.ToString();
                if (_car != null) 
                {
                    if (_car.Brand != null)
                    {
                        if (!CarBrandSource.Contains(_car.Brand)) CarBrandSource.Add(_car.Brand);
                        CarBrand = _car.Brand;
                    }
                    else if(CarBrand != null) 
                    {
                        _car.Brand = CarBrand;
                    }
                }
                OnPropertyChanged(nameof(Car));
            }
        }

        private void getCars(object e)
        {
            string text = e as string;
            CarSource = _context.Cars
                            .Include(car => car.Brand)
                            .Where(t => t.Number.ToLower().Contains(text.ToLower()))
                            .OrderBy(t => t.Number)
                            .Take(5)
                            .ToList();
        }

        //------------------------ Trailler --------------------------------
        public DelegateCommand GetTraillers { get; private set; }

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
                _accountNameBuilder.Trailler = _trailler;
                AccountName = _accountNameBuilder.ToString();
                if (_trailler != null)
                {
                    if (_trailler.Brand != null)
                    {
                        if (!TraillerBrandSource.Contains(_trailler.Brand)) TraillerBrandSource.Add(_trailler.Brand);
                        TraillerBrand = _trailler.Brand;
                    }
                    else if(TraillerBrand != null)
                    {
                        _trailler.Brand = TraillerBrand;
                    }
                    
                }
                OnPropertyChanged(nameof(Trailler));
            }
        }

        private void getTraillers(object e)
        {
            string text = e as string;
            TraillerSource = _context.Traillers
                            .Include(trailler => trailler.Brand)
                            .Where(t => t.Number.ToLower().Contains(text.ToLower()))
                            .OrderBy(t => t.Number)
                            .Take(5)
                            .ToList();
        }
    }
}
