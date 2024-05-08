using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class RussianBrandName
    {
        public int RussianBrandNameId { get; set; }
        public string Name { get; set; }
        public int CarBrandId { get; set; }
        public CarBrand СarBrand { get; set; }
    }
}
