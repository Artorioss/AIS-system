using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Car : IEntity
    {
        public Car() 
        {
            Drivers = new ObservableCollection<Driver>();
            Transportations = new ObservableCollection<Transportation>();
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
        public ObservableCollection<Driver> Drivers { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }

        public void SetFields(IEntity car) 
        {
            Car entityCar = (Car)car;
            Number = entityCar.Number;
            BrandId = entityCar.BrandId;
            IsTruck = entityCar.IsTruck;
            Brand = entityCar.Brand;
            SoftDeleted = entityCar.SoftDeleted;
            Drivers = new ObservableCollection<Driver>();
            Transportations = new ObservableCollection<Transportation>();
            if (entityCar.Drivers != null && entityCar.Drivers.Count > 0) foreach (var driver in entityCar.Drivers) Drivers.Add(driver);
            if (entityCar.Transportations != null && entityCar.Transportations.Count > 0) foreach(var transportation in entityCar.Transportations) Transportations.Add(transportation);
        }

        public object Clone()
        {
            return new Car(this);
        }
    }
}
