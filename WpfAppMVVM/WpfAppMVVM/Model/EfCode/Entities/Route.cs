using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Route: ICloneable
    {
        public int RouteId { get; set; }
        [MaxLength(128)]
        public string RouteName { get; set; }
        public ICollection<Transportation> Transportations { get; set; }
        public ICollection<RoutePoint> RoutePoints { get; set; }

        public Route() 
        {
            Transportations = new HashSet<Transportation>();
            RoutePoints = new HashSet<RoutePoint>();
        }

        public Route(Route route)
        {
            SetFields(route);
        }

        public void SetFields(Route route) 
        {
            RouteId = route.RouteId;
            RouteName = route.RouteName;
            Transportations = route.Transportations;
            RoutePoints = route.RoutePoints;
        }

        public object Clone()
        {
            return new Route(this);
        }
    }
}
