﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models;
using WpfAppMVVM.Model.Command;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Runtime.Serialization;
using WpfAppMVVM.Model;
using System.Net;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        public Transportation Transportation { get; set; }
        protected RoutePointBuilder RoutePointLoader { get; set; }
        private AccountNameBuilder _accountNameBuilder { get; set; }
        public DelegateCommand CreateTransportation { get; private set; }
        public DelegateCommand Loaded { get; private set; }
        public bool IsContextChanged { get; private set; } = false;

        public CreatingTransportationViewModel()
        {
            settingsUp();
            Transportation = new Transportation();
            _context.Add(Transportation);
        }

        public CreatingTransportationViewModel(int transportationId)
        {
            settingsUp();
            Transportation = _context.Transportations
                                     .Include(transp => transp.Driver)
                                     .ThenInclude(driver => driver.TransportCompany)
                                     .Include(transp => transp.Customer)
                                     .Include(transp => transp.Route)
                                     .Include(transp => transp.Car)
                                     .ThenInclude(car => car.Brand)
                                     .Include(transp => transp.Trailler)
                                     .ThenInclude(trailler => trailler.Brand)
                                     .SingleOrDefault(s => s.TransportationId == transportationId);

            WindowName = "Редактирование заявки";
            ButtonName = "Сохранить изменения";
            setFields();
        }

        private void settingsUp() 
        {
            CustomerSource = new List<Customer>();
            DriversSource = new List<Driver>();
            CompaniesSource = new List<TransportCompany>();
            CarBrandSource = new List<CarBrand>();
            CarSource = new List<Car>();
            TraillerSource = new List<Trailler>();
            TraillerBrandSource = new List<TraillerBrand>();
            GetCarBrands = new DelegateCommand(getCarBrands);
            GetTraillerBrands = new DelegateCommand(getTraillerBrands);
            GetCustomers = new DelegateCommand(getCustomers);
            GetDrivers = new DelegateCommand(getDrivers);
            GetCompanies = new DelegateCommand(getCompanies);
            GetCars = new DelegateCommand(getCars);
            GetTraillers = new DelegateCommand(getTraillers);
            RoutePointSource = new List<RoutePoint>();
            AddLoadingRoute = new DelegateCommand((obj) => AddLoadingRoutePoint());
            AddLoadingRouteByKeyboard = new DelegateCommand(RouteLoading_KeyDown);
            AddDispatcherRoute = new DelegateCommand((obj) => AddDispatcherRoutePoint());
            AddDispatcherRouteByKeyboard = new DelegateCommand(RouteDispatcher_KeyDown);
            GetPointRouteLoadings = new DelegateCommand(getPointRouteLoadings);
            GetPointRouteDispatchers = new DelegateCommand(getPointRouteDispatchers);
            CreateTransportation = new DelegateCommand(createTransportation);
            _routePointBuilder = new RoutePointBuilder();
            _accountNameBuilder = new AccountNameBuilder();
        }

        private void setFields() 
        {
            CustomerSource = new List<Customer>() { Transportation.Customer };
            Customer = Transportation.Customer;
            DateTime = (DateTime)Transportation.DateLoading;
            Payment = Transportation.Price == null ? 0.00M : (decimal)Transportation.Price;
            GeneralRoute = Transportation.Route.RouteName;

            DriversSource = new List<Driver>() { Transportation.Driver };
            _driver = Transportation.Driver;
            OnPropertyChanged(nameof(Driver));

            if (_driver != null && _driver.TransportCompany != null) 
            {
                CompaniesSource = new List<TransportCompany>() { Transportation.Driver.TransportCompany };
                _company = Transportation.Driver.TransportCompany;
            } 
            else CompaniesSource = new List<TransportCompany>();         
            OnPropertyChanged(nameof(TransportCompany));

            CarSource = new List<Car>();
            if (Transportation.Car != null) 
            {
                CarSource.Add(Transportation.Car);
                _car = Transportation.Car;
                OnPropertyChanged(nameof(Car));

                CarBrandSource = new List<CarBrand>();
                if (Transportation.Car.Brand != null) 
                {
                    CarBrandSource.Add(Transportation.Car.Brand);
                    _carBrand = Transportation.Car.Brand;
                    OnPropertyChanged(nameof(CarBrand));
                }
            }

            TraillerSource = new List<Trailler>();
            if (Transportation.Trailler != null) 
            {
                TraillerSource.Add(Transportation.Trailler);
                _trailler = Transportation.Trailler;
                OnPropertyChanged(nameof(Trailler));

                TraillerBrandSource = new List<TraillerBrand>();
                if (Transportation.Trailler.Brand != null) 
                {
                    TraillerBrandSource.Add(Transportation.Trailler.Brand);
                    _traillerBrand = Transportation.Trailler.Brand;
                    OnPropertyChanged(nameof(TraillerBrand));
                }
            }

            PayToDriver = Transportation.PaymentToDriver == null ? 0.00M : (decimal)Transportation.PaymentToDriver;

            _accountNameBuilder.Driver = _driver;
            _accountNameBuilder.CarBrand = _carBrand;
            _accountNameBuilder.Car = _car;
            _accountNameBuilder.TraillerBrand = _traillerBrand;
            _accountNameBuilder.Trailler = _trailler;
            _accountNameBuilder.Date = DateTime;

            AccountName = Transportation.RouteName;
        }

        string _accountName;
        public string AccountName 
        {
            get => _accountName;
            set 
            {
                _accountName = value;
                //_accountNameBuilder.setRoutName(value); //TODO
                OnPropertyChanged(nameof(AccountName));
            }
        }

        DateTime _dateTime = DateTime.Now;
        public DateTime DateTime 
        {
            get => _dateTime;
            set 
            {
                _dateTime = value;
                _accountNameBuilder.Date = value;
                AccountName = _accountNameBuilder.ToString();
                OnPropertyChanged(nameof(DateTime));
            }
        }

        private decimal _payment = 0.00M;
        public decimal Payment 
        {
            get => _payment;
            set 
            {
                _payment = value; 
                OnPropertyChanged(nameof(Payment));
            }
        }

        private decimal _payToDriver = 0.00M;
        public decimal PayToDriver 
        {
            get => _payToDriver;
            set 
            {
                _payToDriver = value;
                OnPropertyChanged(nameof(PayToDriver));
            }
        }

        private string _windowName = "Создание заявки";
        public string WindowName 
        {
            get => _windowName;
            set 
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        private string _buttonName = "Создать заявку";
        public string ButtonName 
        {
            get => _buttonName;
            set 
            {
                _buttonName = value;
                OnPropertyChanged(nameof(ButtonName));
            }
        }

        private void createTransportation(object obj)
        {
            if (Customer is null)
            {
                MessageBox.Show("Укажите заказчика.", "Неверно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Route route = CreateRoute();
            if (route is null) 
            {
                MessageBox.Show("Укажите маршрут.", "Неверно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Driver != null) 
            {
                if (Car != null) 
                {
                    _context.Entry(Driver).Collection(d => d.Cars).Load();
                    if (Driver.Cars.SingleOrDefault(car => car.Number == Car.Number, null) == null) Driver.Cars.Add(Car);
                }
                if (Trailler != null) 
                {
                    _context.Entry(Driver).Collection(d => d.Traillers).Load();
                    if (Driver.Traillers.SingleOrDefault(trailler => trailler.Number == Trailler.Number, null) == null) Driver.Traillers.Add(Trailler);
                }

                if (Driver.TransportCompanyId != TransportCompany.TransportCompanyId) Driver.TransportCompany = TransportCompany;
            }  

            Transportation.RouteName = AccountName;
            Transportation.DateLoading = DateTime;
            Transportation.Customer = Customer;
            Transportation.Driver = Driver;
            Transportation.Route = route;
            Transportation.Car = Car;
            Transportation.Trailler = Trailler;
            Transportation.Price = Payment;
            Transportation.PaymentToDriver = PayToDriver;
            Transportation.StateOrder = _context.StateOrders.Single(s => s.StateOrderId == 1);
            
            _context.SaveChanges();
            IsContextChanged = true;
            (obj as Window).Close();
        }

        private Route CreateRoute() 
        {
            Route route = _context.Routes.FirstOrDefault(route => route.RouteName == GeneralRoute);
            if (route == null) 
            {
                List<RoutePoint> list = new List<RoutePoint>();
                foreach (var item in _routePointBuilder.getRoutes())
                {
                    var point = _context.RoutePoints.FirstOrDefault(p => p.Name == item.Name);
                    if (point is null) list.Add(item);
                    else list.Add(point);
                }

                if (list.Count > 0) route = new Route()
                {
                    RouteName = GeneralRoute,
                    RoutePoints = list
                };
            }
            return route;
        }
    }
}
