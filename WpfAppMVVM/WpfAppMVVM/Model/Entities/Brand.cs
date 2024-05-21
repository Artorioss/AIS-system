using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Model.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string? RussianBrandName { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
    }

}
