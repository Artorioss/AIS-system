using System.ComponentModel.DataAnnotations;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Model.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string? RussianBrandName { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
    }

}
