using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.Models.Entities
{
    public class Trailler
    {
        public int TraillerId { get; set; }
        public int? BrandId { get; set; }
        public string Number { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}
