using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Transportation
    {
        public int TransportationId { get; set; }
        public DateTime? DateLoading { get; set; }
        public int CustomerId { get; set; }
        public int? DriverId { get; set; }
        public int? RouteId { get; set; }
        [ForeignKey("Car")]
        public string? CarNumber { get; set; }
        [ForeignKey("Trailler")]
        public string? TraillerNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? PaymentToDriver { get; set; }
        public int? PaymentMethodId { get; set; }
        [MaxLength(512)]
        public string RouteName { get; set; }
        public int StateOrderId { get; set; }
        public Car Car { get; set; }
        public Trailler Trailler { get; set; }
        public Route Route { get; set; }
        public Customer Customer { get; set; }
        public Driver Driver { get; set; }
        public StateOrder StateOrder { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
