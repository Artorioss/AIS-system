using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class TraillerBrand
    {
        public int TraillerBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
    }
}
