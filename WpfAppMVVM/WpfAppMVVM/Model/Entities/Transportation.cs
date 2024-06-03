using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("Car")]
        public string? CarNumber { get; set; }
        [ForeignKey("Trailler")]
        public string? TraillerNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? PaymentToDriver { get; set; }
        [MaxLength(512)]
        public string RouteName { get; set; }
        public int StateOrderId { get; set; }
        public Car Car { get; set; }
        public Trailler Trailler { get; set; }
        public Route Route { get; set; }
        public Customer Customer { get; set; }
        public Driver Driver { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public StateOrder StateOrder { get; set; }
    }
}
