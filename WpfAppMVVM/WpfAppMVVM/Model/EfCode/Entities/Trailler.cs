using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Trailler : ICloneable
    {
        [Key, Required, MaxLength(8)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public object Clone()
        {
            Trailler newTrailler = (Trailler)MemberwiseClone();
            if (Brand != null)
            {
                Brand newBrand = new Brand()
                {
                    BrandId = Brand.BrandId,
                    Name = Brand.Name,
                    RussianBrandName = Brand.RussianBrandName
                };
                newTrailler.Brand = newBrand;
            }
            return newTrailler;
        }
    }
}
