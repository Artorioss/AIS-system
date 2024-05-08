using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Car
    {
        public int CarId { get; set; }
        public int? CarBrandId { get; set; }
        public string Number { get; set; }
        public bool IsTruck { get; set; }
        public CarBrand CarBrand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}
