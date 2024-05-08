using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models.Entities;
using WpfAppMVVM.Models;
using System.Windows.Input;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        private RoutePointLoader _routePointLoader;
        public ObservableCollection<RoutePoint> RoutePoints { get; set; }
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
    }
}
