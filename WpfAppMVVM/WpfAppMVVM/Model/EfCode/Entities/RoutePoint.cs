using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class RoutePoint
    {
        public int RoutePointId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public ICollection<Route> Routes { get; set; }
    }
}
