using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.Models.Entities
{
    public class Trailler: ICloneable
    {
        public int TraillerId { get; set; }
        public int? BrandId { get; set; }
        public string Number { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public object Clone()
        {
            Trailler newTrailler = new Trailler();
            newTrailler.TraillerId = TraillerId;
            newTrailler.Number = Number;
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
