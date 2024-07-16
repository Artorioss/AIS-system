using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Transportation: IEntity
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
        public bool SoftDeleted { get; set; }
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

        public void SetFields(IEntity transportation) 
        {
            Transportation transportationEntity = transportation as Transportation;
            TransportationId = transportationEntity.TransportationId;
            DateLoading = transportationEntity.DateLoading;
            CustomerId = transportationEntity.CustomerId;
            DriverId = transportationEntity.DriverId;
            RouteId = transportationEntity.RouteId;
            CarNumber = transportationEntity.CarNumber;
            TraillerNumber = transportationEntity.TraillerNumber;
            Price = transportationEntity.Price;
            PaymentToDriver = transportationEntity.PaymentToDriver;
            PaymentMethodId = transportationEntity.PaymentMethodId;
            RouteName = transportationEntity.RouteName;
            StateOrderId = transportationEntity.StateOrderId;
            Car = transportationEntity.Car;
            Trailler = transportationEntity.Trailler;
            Route = transportationEntity.Route;
            Customer = transportationEntity.Customer;
            Driver = transportationEntity.Driver;
            StateOrder = transportationEntity.StateOrder;
            PaymentMethod = transportationEntity.PaymentMethod;
            SoftDeleted = transportationEntity.SoftDeleted;
        }
    }
}
