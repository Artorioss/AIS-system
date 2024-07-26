using System;
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
using WpfAppMVVM.ViewModels.OtherViewModels;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel: BaseViewModel
    {
        public Transportation Transportation { get; set; }
        private AccountNameBuilder _accountNameBuilder { get; set; }

        public CreatingTransportationViewModel()
        {
            Transportation = new Transportation();
            Transportation.StateOrder = _context.StateOrders.Single(s => s.StateOrderId == 1);
            mode = Mode.Additing;
            settingsUp();
        }

        public CreatingTransportationViewModel(Transportation transportation)
        {
            Transportation = transportation;
            WindowName = "Редактирование заявки";
            ButtonName = "Сохранить изменения";
            mode = Mode.Editing;
            settingsUp();
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
            RoutePointSource = new List<RoutePoint>();
            _routePointBuilder = new RoutePointBuilder();
            _accountNameBuilder = new AccountNameBuilder();
        }

        protected override void setCommands() 
        {
            GetCarBrands = new DelegateCommand(getCarBrands);
            GetTraillerBrands = new DelegateCommand(getTraillerBrands);
            GetCustomers = new DelegateCommand(getCustomers);
            GetDrivers = new DelegateCommand(getDrivers);
            GetCompanies = new DelegateCommand(getCompanies);
            GetCars = new DelegateCommand(getCars);
            GetTraillers = new DelegateCommand(getTraillers);
            AddLoadingRoute = new AsyncCommand(async (obj) => await AddLoadingRoutePoint());
            AddLoadingRouteByKeyboard = new DelegateCommand(RouteLoading_KeyDown);
            AddDispatcherRoute = new AsyncCommand(async (obj) => await AddDispatcherRoutePoint());
            AddDispatcherRouteByKeyboard = new DelegateCommand(RouteDispatcher_KeyDown);
            GetPointRouteLoadings = new DelegateCommand(getPointRouteLoadings);
            GetPointRouteDispatchers = new DelegateCommand(getPointRouteDispatchers);
        }

        private void setFields() 
        {
            CustomerSource.Add(Customer);
            DriversSource.Add(Driver);   
            if (Driver != null && Driver.TransportCompany != null) CompaniesSource.Add(TransportCompany);        

            if (Car != null) 
            {
                CarSource.Add(Car);
                if (Car.Brand != null) 
                {
                    CarBrandSource.Add(Car.Brand);
                }
            }
;
            if (Trailler != null) 
            {
                TraillerSource.Add(Trailler);
                if (TraillerBrand != null) 
                {
                    TraillerBrandSource.Add(TraillerBrand);
                }
            }

            _accountNameBuilder.Driver = Driver;
            _accountNameBuilder.CarBrand = CarBrand;
            _accountNameBuilder.Car = Car;
            _accountNameBuilder.TraillerBrand = TraillerBrand;
            _accountNameBuilder.Trailler = Trailler;
            _accountNameBuilder.Date = DateTime;
        }

        public string AccountName 
        {
            get => Transportation.RouteName;
            set 
            {
                Transportation.RouteName = value;
                OnPropertyChanged(nameof(AccountName));
            }
        }

        public DateTime DateTime 
        {
            get => Transportation.DateLoading.HasValue ? Transportation.DateLoading.Value : DateTime.Now;
            set 
            {
                Transportation.DateLoading = value;
                _accountNameBuilder.Date = value;
                AccountName = _accountNameBuilder.ToString();
                OnPropertyChanged(nameof(DateTime));
            }
        }

        public decimal Payment 
        {
            get => Transportation.Price.HasValue ? Transportation.Price.Value : 0.00M;
            set 
            {
                Transportation.Price = value; 
                OnPropertyChanged(nameof(Payment));
            }
        }

        public decimal PayToDriver 
        {
            get => Transportation.PaymentToDriver.HasValue ? Transportation.PaymentToDriver.Value : 0.00M;
            set 
            {
                Transportation.PaymentToDriver = value;
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

        protected override async Task<bool> dataIsCorrect() 
        {
            if (Customer is null)
            {
                MessageBox.Show("Укажите заказчика.", "Неверно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            Route route = await CreateRoute();
            if (route is null)
            {
                MessageBox.Show("Укажите маршрут.", "Неверно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else Transportation.Route = route;
            return true;
        }

        private async Task<Route> CreateRoute()
        {
            Route route = await _context.Routes.FirstOrDefaultAsync(route => route.RouteName == GeneralRoute);
            if (route == null)
            {
                List<RoutePoint> list = new List<RoutePoint>();
                foreach (var item in _routePointBuilder.getRoutes())
                {
                    var point = await _context.RoutePoints.FirstOrDefaultAsync(p => p.Name == item.Name);
                    if (point is null) list.Add(item);
                    else list.Add(point);
                }

                if (list.Count > 0) route = new Route()
                {
                    RouteName = GeneralRoute,
                    RoutePoints = new ObservableCollection<RoutePoint>(list)
                };
            }
            return route;
        }

        private void setData() // TODO 
        {
            if (Driver != null && Trailler != null)
            {
                if (Driver.Traillers.FirstOrDefault(t => t.Number == Trailler.Number) is null) Driver.Traillers.Add(Trailler);
                Trailler.Drivers.Add(Driver);
            }

            if (Driver != null && Car != null)
            {
                if (Driver.Cars.FirstOrDefault(t => t.Number == Car.Number) is null) Driver.Cars.Add(Car);
                Car.Drivers.Add(Driver);
            }

        }

        protected override void cloneEntity() 
        {
            Transportation = Transportation.Clone() as Transportation;
        }
        protected override async Task loadReferenceData() 
        {
            if(Transportation.Route is null) Transportation.Route = new Route();
        }

        protected override async Task updateEntity() 
        {
            setData();
            var transportation = await _context.Transportations.FindAsync(Transportation.TransportationId);
            transportation.SetFields(Transportation);
        }
        protected override async Task addEntity() 
        {
            setData();
            await _context.Transportations.AddAsync(Transportation);
        }

        public override async Task<IEntity> GetEntity() => await _context.Transportations.FindAsync(Transportation.TransportationId);
    }
}
