using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Route: IEntity
    {
        public int RouteId { get; set; }
        [MaxLength(128)]
        public string RouteName { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Transportation> Transportations { get; set; }
        public ICollection<RoutePoint> RoutePoints { get; set; }

        public Route() 
        {
            Transportations = new List<Transportation>();
            RoutePoints = new List<RoutePoint>();
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
            Transportations = new List<Transportation>();
            RoutePoints = new List<RoutePoint>();
            if (routeEntity.Transportations != null && routeEntity.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(routeEntity.Transportations);
            if (routeEntity.RoutePoints != null && routeEntity.RoutePoints.Count > 0) (RoutePoints as List<RoutePoint>).AddRange(routeEntity.RoutePoints);
            SoftDeleted = routeEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new Route(this);
        }
    }
}
