using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.ViewModels.OtherViewModels;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    public partial class CreatingTransportationViewModel : BaseViewModel
    {
        private RoutePointBuilder _routePointBuilder;
        public List<RoutePoint> _routePointSource;
        public List<RoutePoint> RoutePointSource
        {
            get => _routePointSource;
            set 
            {
                _routePointSource = value;
                OnPropertyChanged(nameof(RoutePointSource));
            }
        }
        public AsyncCommand AddLoadingRoute { get; private set; }
        public DelegateCommand AddLoadingRouteByKeyboard { get; private set; }
        public AsyncCommand AddDispatcherRoute { get; private set; }
        public DelegateCommand AddDispatcherRouteByKeyboard { get; private set; }
        public DelegateCommand GetPointRouteLoadings { get; private set; }
        public DelegateCommand GetPointRouteDispatchers { get; private set; }

        public string GeneralRoute
        {
            get => Transportation.Route != null? Transportation.Route.RouteName : string.Empty;
            set
            {
                Transportation.Route.RouteName = value;
                _routePointBuilder.setRoutePoints(value);
                setAccountName(value);
                OnPropertyChanged(nameof(GeneralRoute));
            }
        }

        private void setAccountName(string val)
        {
            _accountNameBuilder.RouteName = val;
            AccountName = _accountNameBuilder.ToString();
        }

        private string _loadingRoutePointName;
        public string LoadingRoutePointName
        {
            get => _loadingRoutePointName;
            set
            {
                _loadingRoutePointName = value;
                OnPropertyChanged(nameof(LoadingRoutePointName));
                if(LoadingRoutePoint is null) getPointRouteLoadings(_loadingRoutePointName);
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
                if (DispatcherRoutePoint is null) getPointRouteDispatchers(_dispatcherRoutePointName);
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

        private void getPointRouteLoadings(object e)
        {
            string text = e as string;
            RoutePointSource = _context.RoutePoints
                                        .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                        .OrderBy(c => c.Name)
                                        .Take(5)
                                        .ToList();
        }

        private void getPointRouteDispatchers(object e)
        {
            string text = e as string;
            RoutePointSource = _context.RoutePoints
                                        .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                        .OrderBy(c => c.Name)
                                        .Take(5)
                                        .ToList();
        }

        private async Task AddLoadingRoutePoint()
        {
            if (LoadingRoutePoint != null)
            {
                _routePointBuilder.AddLoading(LoadingRoutePoint);
                LoadingRoutePoint = null;
            }
            else if (!string.IsNullOrEmpty(LoadingRoutePointName))
            {
                RoutePoint route_Point = await _context.RoutePoints
                    .Where(s => s.Name.ToLower().Contains(LoadingRoutePointName.ToLower()))
                    .FirstOrDefaultAsync();
                if (route_Point is null)
                    route_Point = new RoutePoint { Name = LoadingRoutePointName };
                _routePointBuilder.AddLoading(route_Point);
            }
            Transportation.Route.RouteName = _routePointBuilder.ToString();
            OnPropertyChanged(nameof(GeneralRoute));
            setAccountName(GeneralRoute);
            LoadingRoutePointName = string.Empty;
        }

        private async Task AddDispatcherRoutePoint()
        {
            if (DispatcherRoutePoint != null)
            {
                _routePointBuilder.AddDispatcher(DispatcherRoutePoint);
                DispatcherRoutePoint = null;
            }
            else if (!string.IsNullOrEmpty(DispatcherRoutePointName))
            {
                RoutePoint route_Point = await _context.RoutePoints
                    .Where(s => s.Name.ToLower().Contains(DispatcherRoutePointName.ToLower()))
                    .FirstOrDefaultAsync();
                if (route_Point is null)
                    route_Point = new RoutePoint { Name = DispatcherRoutePointName };
                _routePointBuilder.AddDispatcher(route_Point);
            }
            Transportation.Route.RouteName = _routePointBuilder.ToString();
            OnPropertyChanged(nameof(GeneralRoute));
            setAccountName(GeneralRoute);
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
    }
}
