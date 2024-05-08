using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.Model.Command;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WpfAppMVVM.ViewModels
{
    internal class CreatingTransportationViewModel : BaseViewModel
    {
        private TransportationEntities _context;
        public Transportation Transportation { get; set; }
        public Driver Driver { get; set; }
        public Car Car { get; set; }
        public Trailler Trailer { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public RoutePointLoader RoutePointLoader { get; set; }

        public CreatingTransportationViewModel()
        {
            _context = (Application.Current as App)._context;
            Customers = new ObservableCollection<Customer>();
            RoutePoints = new ObservableCollection<RoutePoint>();
            SetCustomer = new DelegateCommand((obj) => setCustomer());
            AddLoadingRoute = new DelegateCommand((obj) => AddLoadingRoutePoint());
            AddLoadingRouteByKeyboard = new DelegateCommand(RouteLoading_KeyDown);
            AddDispatcherRoute = new DelegateCommand((obj) => AddDispatcherRoutePoint());
            AddDispatcherRouteByKeyboard = new DelegateCommand(RouteDispatcher_KeyDown);
            _routePointLoader = new RoutePointLoader();
        }

        //--------------------------------- ComboBox Customer ----------------------------------------
        public ObservableCollection<Customer> Customers { get; set; }
        public DelegateCommand SetCustomer { get; set; }
        private bool freezComboBox = false;

        Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
               _customer = value;
                if (freezComboBox) 
                {
                    var cust = Customers.SingleOrDefault(x => x.CustomerId == value.CustomerId);
                    if (cust != null) 
                    {
                        Customers.Remove(cust);
                        Customers.Add(value);
                    }
                    else 
                    {
                        Customers.Clear();
                        Customers.Add(value);
                    }
                }
                OnPropertyChanged(nameof(Customer));
            }
        }

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
                if(!freezComboBox) getCustomers();
            }
        }
        private bool _isDropDownOpenCustomers;
        public bool IsDropDownOpenCustomers
        {
            get => _isDropDownOpenCustomers;
            set
            {
                _isDropDownOpenCustomers = value;
                OnPropertyChanged(nameof(IsDropDownOpenCustomers));
            }
        }
        private void getCustomers()
        {
            if (string.IsNullOrEmpty(CustomerName))
            {
                IsDropDownOpenCustomers = false;
                Customer = null;
            }

            if (Customer == null) 
            {
                var list = _context.Customers.AsNoTracking()
                                .Where(c => c.Name.ToLower().Contains(CustomerName.ToLower()))
                                .OrderBy(c => c.Name)
                                .Take(5);
                Customers.Clear();
                foreach (var item in list)
                {
                    Customers.Add(item);
                }
            } 

            IsDropDownOpenCustomers = Customers.Count > 0;
                       
        }

        private void setCustomer()
        {
            if (_customer == null && !string.IsNullOrEmpty(CustomerName))
            {
                freezComboBox = true;
                Customer cust = _context.Customers
                        .FirstOrDefault(s => s.Name.ToLower() == CustomerName.ToLower());

                if (cust is null)
                    Customer = new Customer { Name = CustomerName };
                else
                {
                    Customer = cust;
                }
                freezComboBox = false;
            }
        }
        //------------------------------- ComboBox Customer ------------------------------



        //------------------------------- ComboBoxes RoutePoints -------------------------
        private RoutePointLoader _routePointLoader;
        public ObservableCollection<RoutePoint> RoutePoints {get; set;}
        public DelegateCommand AddLoadingRoute { get; private set; }
        public DelegateCommand AddLoadingRouteByKeyboard { get; private set; }
        public DelegateCommand AddDispatcherRoute { get; private set; }
        public DelegateCommand AddDispatcherRouteByKeyboard { get; private set; }

        private bool _generalRoutFocusable;
        public bool GeneralRoutFocusable
        {
            get => _generalRoutFocusable;
            set 
            {
                _generalRoutFocusable = value;
                OnPropertyChanged(nameof(GeneralRoutFocusable));
            }
        }

        private string _generalRoute;
        public string GeneralRoute
        {
            get => _generalRoute;
            set
            {
                _generalRoute = value;
                if (_generalRoutFocusable) _routePointLoader.setRoutePoints(value);
                OnPropertyChanged(nameof(GeneralRoute));
            }
        }

        private string _loadingRoutePointName;
        public string LoadingRoutePointName
        {
            get => _loadingRoutePointName;
            set
            {
                _loadingRoutePointName = value;
                OnPropertyChanged(nameof(LoadingRoutePointName));
                getPointRouteLoadings();
            }
        }

        private string _dispatcherRoutePointName;
        public string DispatcherRoutePointName
        {
            get => _dispatcherRoutePointName;
            set
            {
                _dispatcherRoutePointName = value;
                OnPropertyChanged(nameof(DispatcherRoutePointName));
                getPointRouteDispatchers();
            }
        }

        RoutePoint _loadingRoutePoint;
        public RoutePoint LoadingRoutePoint
        {
            get => _loadingRoutePoint;
            set 
            {
                _loadingRoutePoint = value;
                OnPropertyChanged(nameof(LoadingRoutePoint));
            }
        }

        RoutePoint _dispatcherRoutePoint;
        public RoutePoint DispatcherRoutePoint
        {
            get => _dispatcherRoutePoint;
            set
            {
                _dispatcherRoutePoint = value;
                OnPropertyChanged(nameof(DispatcherRoutePoint));
            }
        }

        private bool _isDropDownOpenLoadings;
        public bool IsDropDownOpenLoadings 
        {
            get => _isDropDownOpenLoadings;
            set 
            {
                _isDropDownOpenLoadings = value;
                OnPropertyChanged(nameof(IsDropDownOpenLoadings));
            }
        }

        private bool _isDropDownOpenDispatchers;
        public bool IsDropDownOpenDispatchers
        {
            get => _isDropDownOpenDispatchers;
            set
            {
                _isDropDownOpenDispatchers = value;
                OnPropertyChanged(nameof(IsDropDownOpenDispatchers));
            }
        }

        private void getPointRouteLoadings() 
        {
            if (string.IsNullOrEmpty(LoadingRoutePointName))
            {
                IsDropDownOpenLoadings = false;
                LoadingRoutePoint = null;
            }
            else if (LoadingRoutePoint == null) 
            {
                var list = _context.RoutePoints.AsNoTracking()
                                          .Where(c => c.Name.ToLower().Contains(LoadingRoutePointName.ToLower()))
                                          .OrderBy(c => c.Name)
                                          .Take(5)
                                          .ToList();

                RoutePoints.Clear();
                foreach (var routePoint in list)
                {
                    RoutePoints.Add(routePoint);
                }
            }

            IsDropDownOpenLoadings = RoutePoints.Count > 0;
        }

        private void getPointRouteDispatchers()
        {
            if (string.IsNullOrEmpty(DispatcherRoutePointName))
            {
                IsDropDownOpenDispatchers = false;
                DispatcherRoutePoint = null;
            }
            else if (DispatcherRoutePoint == null)
            {
                var list = _context.RoutePoints.AsNoTracking()
                                          .Where(c => c.Name.ToLower().Contains(DispatcherRoutePointName.ToLower()))
                                          .OrderBy(c => c.Name)
                                          .Take(5)
                                          .ToList();

                RoutePoints.Clear();
                foreach (var routePoint in list)
                {
                    RoutePoints.Add(routePoint);
                }
            }

            IsDropDownOpenDispatchers = RoutePoints.Count > 0;
        }

        private void AddLoadingRoutePoint()
        {
            if (LoadingRoutePoint != null)
            {
                _routePointLoader.AddLoading(LoadingRoutePoint);
                LoadingRoutePoint = null;
                GeneralRoute = _routePointLoader.ToString();
                IsDropDownOpenLoadings = false;
            }
            else if (!string.IsNullOrEmpty(LoadingRoutePointName))
            {
                RoutePoint route_Point = _context.RoutePoints
                    .Where(s => s.Name.ToLower().Contains(LoadingRoutePointName.ToLower()))
                    .FirstOrDefault();
                if (route_Point is null)
                    route_Point = new RoutePoint { Name = LoadingRoutePointName};
                _routePointLoader.AddLoading(route_Point);
                GeneralRoute = _routePointLoader.ToString();
            }
            LoadingRoutePointName = string.Empty;
        }

        private void AddDispatcherRoutePoint()
        {
            if (DispatcherRoutePoint != null)
            {
                _routePointLoader.AddDispatcher(DispatcherRoutePoint);
                DispatcherRoutePoint = null;
                GeneralRoute = _routePointLoader.ToString();
                IsDropDownOpenDispatchers = false;
            }
            else if (!string.IsNullOrEmpty(DispatcherRoutePointName))
            {
                RoutePoint route_Point = _context.RoutePoints
                    .Where(s => s.Name.ToLower().Contains(DispatcherRoutePointName.ToLower()))
                    .FirstOrDefault();
                if (route_Point is null)
                    route_Point = new RoutePoint { Name = DispatcherRoutePointName };
                _routePointLoader.AddDispatcher(route_Point);
                GeneralRoute = _routePointLoader.ToString();
            }
            DispatcherRoutePointName = string.Empty;
        }

        private void RouteLoading_KeyDown(object e)
        { 
            if ((Key)e == Key.Enter) AddLoadingRoutePoint();
        }

        private void RouteDispatcher_KeyDown(object e)
        {
            if ((Key)e == Key.Enter) AddDispatcherRoutePoint();
        }
        //------------------------------- ComboBoxes RoutePoints -------------------------

        //------------------------------- Driver information -------------------------

    }
}
