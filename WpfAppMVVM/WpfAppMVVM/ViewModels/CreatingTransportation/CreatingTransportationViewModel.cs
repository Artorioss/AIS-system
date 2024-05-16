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
using System.Runtime.Serialization;
using WpfAppMVVM.Model.Entities;
using WpfAppMVVM.Model;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        public Transportation Transportation { get; set; }
        protected RoutePointBuilder RoutePointLoader { get; set; }
        AccountNameBuilder _accountNameBuilder { get; set; }
        public DelegateCommand CreateTransportation { get; private set; }

        public CreatingTransportationViewModel()
        {
            CustomerSource = new List<Customer>();
            DriversSource = new List<Driver>();
            CompaniesSource = new List<TransportCompany>();
            CarBrandSource = new List<Brand>();
            CarSource = new List<Car>();
            TraillerSource = new List<Trailler>();
            TraillerBrandSource = new List<Brand>();
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

        string _accountName;
        public string AccountName 
        {
            get => _accountName;
            set 
            {
                _accountName = value;
                OnPropertyChanged(nameof(AccountName));
            }
        }

        DateTime _dateTime;
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

        private void createTransportation(object obj) 
        {
            Transportation = new Transportation()
            {
                DateLoading = DateTime.ToShortDateString(),
                CustomerId = Customer.CustomerId,
                DriverId = Driver.DriverId,
                TransportCompanyId = TransportCompany.TransportCompanyId,
                Price = Payment,
                PaymentToDriver = PayToDriver,
                Address = GeneralRoute,
                StateOrder = _context.StateOrders.Single(s => s.StateOrderId == 1),
            };

            _context.Add(Transportation);
            _context.SaveChanges();
            (obj as Window).Close();
        }
    }
}
