using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Models.Entities
{
    public class TraillerBrand
    {
        public int TraillerBrandId { get; set; }
        public string Name { get; set; }
        public ICollection<RussianBrandName> RussianBrandNames { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
    }
}
