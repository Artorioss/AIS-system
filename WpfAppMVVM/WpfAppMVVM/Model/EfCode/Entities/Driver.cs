using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Driver : IEntity
    {
        public Driver() 
        {
            Cars = new List<Car>();
            Traillers = new List<Trailler>();
            Transportations = new List<Transportation>();
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
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public void SetFields(IEntity driver) 
        {
            Driver driverEntity = driver as Driver;
            DriverId = driverEntity.DriverId;
            Name = driverEntity.Name;
            TransportCompanyId = driverEntity.TransportCompanyId;
            TransportCompany = driverEntity.TransportCompany;
            Cars = new List<Car>();
            Traillers = new List<Trailler>();
            Transportations = new List<Transportation>();
            if (driverEntity.Cars != null && driverEntity.Cars.Count > 0) (Cars as List<Car>).AddRange(driverEntity.Cars);
            if (driverEntity.Traillers != null && driverEntity.Traillers.Count > 0) (Traillers as List<Trailler>).AddRange(driverEntity.Traillers);
            if (driverEntity.Transportations != null && driverEntity.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(driverEntity.Transportations);
            SoftDeleted = driverEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new Driver(this);
        }
    }
}
