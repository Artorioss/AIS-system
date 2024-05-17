using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Route
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public ICollection<Transportation> Transportation { get; set; }
        public ICollection<RoutePoint> RoutePoints { get; set; }
    }
}
