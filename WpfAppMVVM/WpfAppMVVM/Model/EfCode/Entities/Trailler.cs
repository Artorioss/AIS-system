using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Trailler : ICloneable
    {
        [Key, Required, MaxLength(8)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public TraillerBrand Brand { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public Trailler() 
        {
            Drivers = new HashSet<Driver>();
        }

        public Trailler(Trailler trailler)
        {
            SetFields(trailler);
        }

        public void SetFields(Trailler trailler) 
        {
            Number = trailler.Number;
            BrandId = trailler.BrandId;
            Brand = trailler.Brand;
            Drivers = trailler.Drivers;
        }

        public object Clone()
        {
            return new Trailler(this);
        }
    }
}
