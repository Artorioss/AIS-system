using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.Models.Entities
{
    public class Transportation
    {
        public int TransportationId { get; set; }
        public DateTime? DateLoading { get; set; }
        public int CustomerId { get; set; }
        public int? DriverId { get; set; }
        public int? RouteId { get; set; }
        public int? TransportCompanyId { get; set; }
        public int? CarId { get; set; }
        public int? CarBrandId { get; set; }
        public int? TraillerId { get; set; }
        public int? TraillerBrandId { get; set; }
        public decimal? Price { get; set; }
        public decimal? PaymentToDriver { get; set; }
        [MaxLength(512)]
        public string RouteName { get; set; }
        public int StateOrderId { get; set; }
        public Car Car { get; set; }
        public Brand CarBrand { get; set; }
        public Trailler Trailler { get; set; }
        public Brand TraillerBrand { get; set; }
        public Route Route { get; set; }
        public Customer Customer { get; set; }
        public Driver Driver { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public StateOrder StateOrder { get; set; }
    }
}
