using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class RoutePoint: ICloneable
    {
        public RoutePoint() 
        {
            Routes = new List<Route>();
        }

        public RoutePoint(RoutePoint routePoint)
        {
            SetFields(routePoint);
        }

        public int RoutePointId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public ICollection<Route> Routes { get; set; }

        public void SetFields(RoutePoint routePoint) 
        {
            RoutePointId = routePoint.RoutePointId;
            Name = routePoint.Name;
            Routes = routePoint.Routes;
        }

        public object Clone()
        {
            return new RoutePoint(this);
        }
    }
}
