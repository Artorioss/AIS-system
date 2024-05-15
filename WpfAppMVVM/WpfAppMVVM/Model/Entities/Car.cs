using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.Models.Entities
{
    public class Car
    {
        public int CarId { get; set; }
        public int? BrandId { get; set; }
        public string Number { get; set; }
        public bool IsTruck { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}
