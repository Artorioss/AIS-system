using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class RoutePoint: IEntity
    {
        public RoutePoint() 
        {
            Routes = new ObservableCollection<Route>();
        }

        public RoutePoint(RoutePoint routePoint)
        {
            SetFields(routePoint);
        }

        public int RoutePointId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<Route> Routes { get; set; }

        public void SetFields(IEntity routePoint) 
        {
            RoutePoint routePointEntity = routePoint as RoutePoint;
            RoutePointId = routePointEntity.RoutePointId;
            Name = routePointEntity.Name;
            Routes = new ObservableCollection<Route>();
            if (routePointEntity.Routes != null && routePointEntity.Routes.Count > 0) foreach(var item in routePointEntity.Routes) Routes.Add(item);
            SoftDeleted = routePointEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new RoutePoint(this);
        }
    }
}
