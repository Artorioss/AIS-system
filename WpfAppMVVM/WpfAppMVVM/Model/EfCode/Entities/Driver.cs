using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Driver : ICloneable
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
        public TransportCompany TransportCompany { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public void SetFields(Driver driver) 
        {
            DriverId = driver.DriverId;
            Name = driver.Name;
            TransportCompanyId = driver.TransportCompanyId;
            TransportCompany = driver.TransportCompany;
            Cars = driver.Cars;
            Traillers = driver.Traillers;
            Transportations = driver.Transportations;
        }

        public object Clone()
        {
            return new Driver(this);
        }
    }
}
