using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Car : IEntity
    {
        public Car() 
        {
            Drivers = new List<Driver>();
            Transportations = new List<Transportation>();
        }

        public Car(Car car)
        {
            SetFields(car);
        }

        [Key, Required, MaxLength(9)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool IsTruck { get; set; }
        public bool SoftDeleted { get; set; }
        public CarBrand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public void SetFields(IEntity car) 
        {
            Car entityCar = (Car)car;
            Number = entityCar.Number;
            BrandId = entityCar.BrandId;
            IsTruck = entityCar.IsTruck;
            Brand = entityCar.Brand;
            SoftDeleted = entityCar.SoftDeleted;
            Drivers = new List<Driver>();
            Transportations = new List<Transportation>();
            if (entityCar.Drivers != null && entityCar.Drivers.Count > 0) (Drivers as List<Driver>).AddRange(entityCar.Drivers);
            if (entityCar.Transportations != null && entityCar.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(entityCar.Transportations);
        }

        public object Clone()
        {
            return new Car(this);
        }
    }
}
