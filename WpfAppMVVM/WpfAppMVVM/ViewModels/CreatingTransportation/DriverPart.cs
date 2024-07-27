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
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.ViewModels.OtherViewModels;

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

        public Driver Driver
        {
            get => Transportation.Driver;
            set
            {
                Transportation.Driver = value;
                _accountNameBuilder.Driver = value;
                AccountName = _accountNameBuilder.ToString();
                if (Transportation.Driver != null)
                {
                    setReferenceData();
                }
                OnPropertyChanged(nameof(Driver));
            }
        }

        private async Task setReferenceData() 
        {
           await setTransportCompanyByDriver();
           await setCarByDriver();
           await setTraillerByDriver();
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

        private async Task setTransportCompanyByDriver()
        {
            var val = await _context.TransportCompanies.FindAsync(Driver.TransportCompanyId);

            if (val != null)
            {
                CompaniesSource = new List<TransportCompany>();
                CompaniesSource.Add(val);
                TransportCompany = val;
            }
        }

        private async Task setCarByDriver()
        {
            await loadReferenceCarData();

            CarSource = Driver.Cars.ToList();

            if (CarSource != null && CarSource.Count > 0)
            {
                CarBrandSource = CarSource.Select(car => car.Brand).Distinct(new CarBrandComparer()).ToList();
                Car = CarSource.FirstOrDefault();
            }
        }

        private async Task loadReferenceCarData() 
        {
            if (!_context.Entry(Driver).Collection(d => d.Cars).IsLoaded)
            {
                await _context.Entry(Driver).Collection(d => d.Cars).LoadAsync();
                foreach (var car in Driver.Cars)
                {
                    await _context.Entry(car).Reference(c => c.Brand).LoadAsync();
                }
            }
        }

        private async Task setTraillerByDriver()
        {
            await loadReferenceTraillerData();

            TraillerSource = Driver.Traillers.ToList();

            if (TraillerSource != null && TraillerSource.Count > 0)
            {
                TraillerBrandSource = TraillerSource.Select(trailler => trailler.Brand).Distinct(new TraillerBrandComparer()).ToList();
                Trailler = TraillerSource.FirstOrDefault();
            }
        }

        private async Task loadReferenceTraillerData()
        {
            if (!_context.Entry(Driver).Collection(d => d.Traillers).IsLoaded)
            {
                await _context.Entry(Driver).Collection(d => d.Traillers).LoadAsync();
                foreach (var trailler in Driver.Traillers)
                {
                    await _context.Entry(trailler).Reference(c => c.Brand).LoadAsync();
                }
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

        public TransportCompany TransportCompany
        {
            get => Transportation.Driver != null ? Transportation.Driver.TransportCompany : null;
            set
            {
                Transportation.Driver.TransportCompany = value;
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

        public CarBrand CarBrand
        {
            get => Car != null ? Car.Brand : null;
            set
            {
                if (Car != null) 
                {
                    Car.Brand = value;
                    _accountNameBuilder.CarBrand = value;
                    AccountName = _accountNameBuilder.ToString();
                }
                OnPropertyChanged(nameof(CarBrand));
            }
        }

        private void getCarBrands(object e)
        {
            string text = e as string;
            CarBrandSource = _context.CarBrands
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()) || c.RussianName.ToLower().Contains(text.ToLower()))
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

        public TraillerBrand TraillerBrand 
        {
            get =>  Trailler != null ? Trailler.Brand : null;
            set 
            {
                if (Trailler != null) 
                {
                    Trailler.Brand = value;
                    _accountNameBuilder.TraillerBrand = value;
                    AccountName = _accountNameBuilder.ToString();
                    OnPropertyChanged(nameof(TraillerBrand));
                }
            }
        }

        private void getTraillerBrands(object e) 
        {
            string text = e as string;
            TraillerBrandSource = _context.TraillerBrands
                                  .Where(tb => (tb.Name.ToLower().Contains(text.ToLower()) || tb.RussianName.ToLower().Contains(text.ToLower())))
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

        public Car Car 
        {
            get => Transportation.Car;
            set 
            {
                Transportation.Car = value;
                _accountNameBuilder.Car = value;
                AccountName = _accountNameBuilder.ToString();
                if (Transportation.Car != null)
                {
                    if (Transportation.Car.Brand != null)
                    {
                        if (!CarBrandSource.Contains(Transportation.Car.Brand)) CarBrandSource.Add(Transportation.Car.Brand);
                        CarBrand = Transportation.Car.Brand;
                    }
                    else if (CarBrand != null)
                    {
                        Transportation.Car.Brand = CarBrand;
                    }
                }
                else OnPropertyChanged(nameof(CarBrand));
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

        public Trailler Trailler 
        {
            get => Transportation.Trailler;
            set 
            {
                Transportation.Trailler = value;
                _accountNameBuilder.Trailler = value;
                AccountName = _accountNameBuilder.ToString();
                if (Transportation.Trailler != null)
                {
                    if (Transportation.Trailler.Brand != null)
                    {
                        if (!TraillerBrandSource.Contains(Transportation.Trailler.Brand)) TraillerBrandSource.Add(Transportation.Trailler.Brand);
                        TraillerBrand = Transportation.Trailler.Brand;
                    }
                    else if (TraillerBrand != null)
                    {
                        Transportation.Trailler.Brand = TraillerBrand;
                    }
                }
                else OnPropertyChanged(nameof(TraillerBrand));
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

        //------------------------ PaymentMethod --------------------------------

        public DelegateCommand GetPaymentMethods { get; private set; }

        private List<PaymentMethod> _paymentMethodsSource;
        public List<PaymentMethod> PaymentMethodsSource
        {
            get => _paymentMethodsSource;
            set
            {
                _paymentMethodsSource = value;
                OnPropertyChanged(nameof(PaymentMethodsSource));
            }
        }

        public PaymentMethod PaymentMethod
        {
            get => Transportation.PaymentMethod;
            set
            {
                Transportation.PaymentMethod = value;
                OnPropertyChanged(nameof(PaymentMethod));
            }
        }

        private void getPaymentMethods(object e)
        {
            string text = e as string;
            PaymentMethodsSource = _context.PaymentMethods
                            .Where(pm => pm.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(t => t.Name)
                            .Take(5)
                            .ToList();
        }
    }
}
