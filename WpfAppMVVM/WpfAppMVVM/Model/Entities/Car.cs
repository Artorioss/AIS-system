using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.Models.Entities
{
    public class Car: ICloneable
    {
        [Key, Required, MaxLength(9)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool IsTruck { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public object Clone()
        {
            Car newCar = (Car)MemberwiseClone();
            if (Brand != null) 
            {
                Brand newBrand = new Brand();
                newBrand.Name = Brand.Name;
                newBrand.BrandId = Brand.BrandId;
                newBrand.RussianBrandName = Brand.RussianBrandName;
                newCar.Brand = newBrand;
            }
            return newCar;
        }
    }
}
