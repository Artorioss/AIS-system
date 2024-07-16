using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class TransportCompany: IEntity
    {
        public int TransportCompanyId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public TransportCompany() 
        {
            Drivers = new List<Driver>();
        }

        public TransportCompany(TransportCompany transportCompany)
        {
            SetFields(transportCompany);
        }

        public void SetFields(IEntity transportCompany) 
        {
            TransportCompany transportCompanyEntity = transportCompany as TransportCompany;
            TransportCompanyId = transportCompanyEntity.TransportCompanyId;
            Name = transportCompanyEntity.Name;
            Drivers = new List<Driver>();
            if (transportCompanyEntity.Drivers != null && transportCompanyEntity.Drivers.Count > 0) (Drivers as List<Driver>).AddRange(transportCompanyEntity.Drivers);
            SoftDeleted = transportCompanyEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new TransportCompany(this);
        }
    }
}
