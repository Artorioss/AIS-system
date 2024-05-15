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

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        public Transportation Transportation { get; set; }
        protected RoutePointLoader RoutePointLoader { get; set; }

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
            GetTraillerBrands = new DelegateCommand(getTraillers);
            GetCustomers = new DelegateCommand(getCustomers);
            GetDrivers = new DelegateCommand(getDrivers);
            GetCompanies = new DelegateCommand(getCompanies);
            //GetCars = new DelegateCommand(getCars);
            RoutePointSource = new List<RoutePoint>();
            AddLoadingRoute = new DelegateCommand((obj) => AddLoadingRoutePoint());
            AddLoadingRouteByKeyboard = new DelegateCommand(RouteLoading_KeyDown);
            AddDispatcherRoute = new DelegateCommand((obj) => AddDispatcherRoutePoint());
            AddDispatcherRouteByKeyboard = new DelegateCommand(RouteDispatcher_KeyDown);
            GetPointRouteLoadings = new DelegateCommand(getPointRouteLoadings);
            GetPointRouteDispatchers = new DelegateCommand(getPointRouteDispatchers);
            _routePointLoader = new RoutePointLoader();
        }
    }
}
