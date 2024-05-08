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
        public int? CarBrandId { get; set; }
        public string Number { get; set; }
        public CarBrand CarBrand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}
