using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Car : ICloneable
    {

        public Car() 
        {
            Drivers = new List<Driver>();
        }

        public Car(Car car)
        {
            setFields(car);
        }

        [Key, Required, MaxLength(9)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool IsTruck { get; set; }
        public CarBrand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public void setFields(Car car) 
        {
            Number = car.Number;
            BrandId = car.BrandId;
            IsTruck = car.IsTruck;
            Brand = car.Brand;
            Drivers = new List<Driver>(car.Drivers);
        }

        public object Clone()
        {
            return new Car(this);
        }
    }
}
