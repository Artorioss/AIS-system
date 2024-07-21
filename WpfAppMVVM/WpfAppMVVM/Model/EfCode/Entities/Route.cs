using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Route: IEntity
    {
        public int RouteId { get; set; }
        [MaxLength(128)]
        public string RouteName { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }
        public ObservableCollection<RoutePoint> RoutePoints { get; set; }

        public Route() 
        {
            Transportations = new ObservableCollection<Transportation>();
            RoutePoints = new ObservableCollection<RoutePoint>();
        }

        public Route(Route route)
        {
            SetFields(route);
        }

        public void SetFields(IEntity route) 
        {
            Route routeEntity = route as Route;
            RouteId = routeEntity.RouteId;
            RouteName = routeEntity.RouteName;
            Transportations = new ObservableCollection<Transportation>();
            RoutePoints = new ObservableCollection<RoutePoint>();
            if (routeEntity.Transportations != null && routeEntity.Transportations.Count > 0) foreach(var transportation in routeEntity.Transportations) Transportations.Add(transportation);
            if (routeEntity.RoutePoints != null && routeEntity.RoutePoints.Count > 0) foreach(var routePoint in routeEntity.RoutePoints) RoutePoints.Add(routePoint);
            SoftDeleted = routeEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new Route(this);
        }
    }
}
