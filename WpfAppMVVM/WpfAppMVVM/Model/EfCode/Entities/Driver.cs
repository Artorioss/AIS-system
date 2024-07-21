using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Driver : IEntity
    {
        public Driver() 
        {
            Cars = new ObservableCollection<Car>();
            Traillers = new ObservableCollection<Trailler>();
            Transportations = new ObservableCollection<Transportation>();
        }

        public Driver(Driver driver)
        {
            SetFields(driver);
        }

        public int DriverId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public int TransportCompanyId { get; set; }
        public bool SoftDeleted { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public ObservableCollection<Car> Cars { get; set; }
        public ObservableCollection<Trailler> Traillers { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }

        public void SetFields(IEntity driver) 
        {
            Driver driverEntity = driver as Driver;
            DriverId = driverEntity.DriverId;
            Name = driverEntity.Name;
            TransportCompanyId = driverEntity.TransportCompanyId;
            TransportCompany = driverEntity.TransportCompany;
            Cars = new ObservableCollection<Car>();
            Traillers = new ObservableCollection<Trailler>();
            Transportations = new ObservableCollection<Transportation>();
            if (driverEntity.Cars != null && driverEntity.Cars.Count > 0) foreach(var car in driverEntity.Cars) Cars.Add(car);
            if (driverEntity.Traillers != null && driverEntity.Traillers.Count > 0) foreach (var trailler in driverEntity.Traillers) Traillers.Add(trailler);
            if (driverEntity.Transportations != null && driverEntity.Transportations.Count > 0) foreach (var transportation in driverEntity.Transportations) Transportations.Add(transportation);
            SoftDeleted = driverEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new Driver(this);
        }
    }
}
