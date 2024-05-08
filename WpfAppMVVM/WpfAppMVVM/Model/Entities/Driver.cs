using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public int TransportCompanyId { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
        public Transportation Transportation { get; set; }
    }
}
