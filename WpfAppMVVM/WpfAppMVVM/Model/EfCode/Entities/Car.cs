﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Car : ICloneable
    {
        [Key, Required, MaxLength(9)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool IsTruck { get; set; }
        public CarBrand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public object Clone()
        {
            Car newCar = (Car)MemberwiseClone();
            if (Brand != null)
            {
                CarBrand newBrand = new CarBrand();
                newBrand.Name = Brand.Name;
                newBrand.CarBrandId = Brand.CarBrandId;
                newBrand.RussianName = Brand.RussianName;
                newCar.Brand = newBrand;
            }
            return newCar;
        }
    }
}
