using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
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
