using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Transportation: ICloneable
    {
        public Transportation() {}

        public Transportation(Transportation transportation)
        {
            SetFields(transportation);
        }

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

        public object Clone()
        {
            return new Transportation(this);
        }

        public void SetFields(Transportation transportation) 
        {
            TransportationId = transportation.TransportationId;
            DateLoading = transportation.DateLoading;
            CustomerId = transportation.CustomerId;
            DriverId = transportation.DriverId;
            RouteId = transportation.RouteId;
            CarNumber = transportation.CarNumber;
            TraillerNumber = transportation.TraillerNumber;
            Price = transportation.Price;
            PaymentToDriver = transportation.PaymentToDriver;
            PaymentMethodId = transportation.PaymentMethodId;
            RouteName = transportation.RouteName;
            StateOrderId = transportation.StateOrderId;
            Car = transportation.Car;
            Trailler = transportation.Trailler;
            Route = transportation.Route;
            Customer = transportation.Customer;
            Driver = transportation.Driver;
            StateOrder = transportation.StateOrder;
            PaymentMethod = transportation.PaymentMethod;
        }
    }
}
