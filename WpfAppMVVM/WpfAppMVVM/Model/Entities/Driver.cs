using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Driver: ICloneable
    {
        public int DriverId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public int TransportCompanyId { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trailler> Traillers { get; set; }
        public ICollection<Transportation> Transportation { get; set; }

        public object Clone()
        {
            Driver newDriver = MemberwiseClone() as Driver;
            if (TransportCompany != null) 
            {
                TransportCompany newTransportCompany = new TransportCompany() 
                {
                    TransportCompanyId = TransportCompany.TransportCompanyId,
                    Name = TransportCompany.Name,
                };
                newDriver.TransportCompany = newTransportCompany;
            }

            if (Cars != null && Cars.Count > 0) 
            {
                var newCars = new List<Car>();

                foreach (var item in Cars)
                {
                    newCars.Add(item.Clone() as Car);
                }

                newDriver.Cars = newCars;
            }

            if (Traillers != null && Traillers.Count > 0) 
            {
                var newTraillers = new List<Trailler>();

                foreach (var item in Traillers) 
                {
                    newTraillers.Add(item.Clone() as Trailler);
                }

                newDriver.Traillers = newTraillers;
            }

            return newDriver;
        }
    }
}
