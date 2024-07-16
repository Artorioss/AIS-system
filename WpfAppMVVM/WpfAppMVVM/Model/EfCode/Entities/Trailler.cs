using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Trailler : IEntity
    {
        [Key, Required, MaxLength(8)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool SoftDeleted { get; set; }
        public TraillerBrand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public Trailler() 
        {
            Drivers = new HashSet<Driver>();
            Transportations = new HashSet<Transportation>();
        }

        public Trailler(Trailler trailler)
        {
            SetFields(trailler);
        }

        public void SetFields(IEntity trailler) 
        {
            Trailler traiilerEntity = trailler as Trailler;
            Number = traiilerEntity.Number;
            BrandId = traiilerEntity.BrandId;
            Brand = traiilerEntity.Brand;
            Drivers = new List<Driver>();
            Transportations = new List<Transportation>();
            if (traiilerEntity.Drivers != null && traiilerEntity.Drivers.Count > 0) (Drivers as List<Driver>).AddRange(traiilerEntity.Drivers);
            if (traiilerEntity.Transportations != null && traiilerEntity.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(traiilerEntity.Transportations);
            SoftDeleted = traiilerEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new Trailler(this);
        }
    }
}
