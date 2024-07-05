using System.ComponentModel.DataAnnotations;

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
            SetFields(car);
        }

        [Key, Required, MaxLength(9)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool IsTruck { get; set; }
        public CarBrand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public void SetFields(Car car) 
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
