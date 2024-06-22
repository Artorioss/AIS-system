using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class CarBrand
    {
        public int CarBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}
