using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Trailler
    {
        public int TraillerId { get; set; }
        public int? TraillerBrandId { get; set; }
        public string Number { get; set; }
        public TraillerBrand TraillerBrand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}
