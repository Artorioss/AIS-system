using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        private RoutePointLoader _routePointLoader;
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
        public DelegateCommand AddLoadingRoute { get; private set; }
        public DelegateCommand AddLoadingRouteByKeyboard { get; private set; }
        public DelegateCommand AddDispatcherRoute { get; private set; }
        public DelegateCommand AddDispatcherRouteByKeyboard { get; private set; }
        public DelegateCommand GetPointRouteLoadings { get; private set; }
        public DelegateCommand GetPointRouteDispatchers { get; private set; }

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
                getPointRouteLoadings(_loadingRoutePointName);
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
                getPointRouteDispatchers(_dispatcherRoutePointName);
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
            RoutePointSource = _context.RoutePoints.AsNoTracking()
                                        .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                        .OrderBy(c => c.Name)
                                        .Take(5)
                                        .ToList();
        }

        private void getPointRouteDispatchers(object e)
        {
            string text = e as string;
            RoutePointSource = _context.RoutePoints.AsNoTracking()
                                        .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                        .OrderBy(c => c.Name)
                                        .Take(5)
                                        .ToList();
        }

        private void AddLoadingRoutePoint()
        {
            if (LoadingRoutePoint != null)
            {
                _routePointLoader.AddLoading(LoadingRoutePoint);
                LoadingRoutePoint = null;
                GeneralRoute = _routePointLoader.ToString();
            }
            else if (!string.IsNullOrEmpty(LoadingRoutePointName))
            {
                RoutePoint route_Point = _context.RoutePoints
                    .Where(s => s.Name.ToLower().Contains(LoadingRoutePointName.ToLower()))
                    .FirstOrDefault();
                if (route_Point is null)
                    route_Point = new RoutePoint { Name = LoadingRoutePointName };
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
    }
}
