using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class TransportCompany: ICloneable
    {
        public int TransportCompanyId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public TransportCompany() 
        {
            Drivers = new HashSet<Driver>();
        }

        public TransportCompany(TransportCompany transportCompany)
        {
            SetFields(transportCompany);
        }

        public void SetFields(TransportCompany transportCompany) 
        {
            TransportCompanyId = transportCompany.TransportCompanyId;
            Name = transportCompany.Name;
            Drivers = transportCompany.Drivers;
        }

        public object Clone()
        {
            return new TransportCompany(this);
        }
    }
}
