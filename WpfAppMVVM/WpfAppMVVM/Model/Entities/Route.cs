using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Route
    {
        public int RouteId { get; set; }
        [MaxLength(128)]
        public string RouteName { get; set; }
        public ICollection<Transportation> Transportation { get; set; }
        public ICollection<RoutePoint> RoutePoints { get; set; }
    }
}
