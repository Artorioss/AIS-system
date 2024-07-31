using WpfAppMVVM.Model.EfCode.Entities;

namespace Test.TestData
{
    internal class TransportationData: ITestData
    {
        public ICollection<IEntity> Entities { get; set; }

        public TransportationData()
        {
            Entities = new List<IEntity>()
            {
                new Transportation()
                {
                    TransportationId = 1,
                    DateLoading = DateTime.UtcNow,
                    Price = 1000.00M,
                    PaymentToDriver = 500.00M,
                    CarNumber = "П148АВ77",
                    TraillerNumber = "TR1234",
                    RouteName = "TEST ROUTE NAME",
                    DriverId = 1,
                    PaymentMethodId = 1,
                    CustomerId = 1,
                    RouteId = 1,
                    StateOrderId = 1
                    
                },
                new Transportation()
                {
                    TransportationId = 2,
                    DateLoading = DateTime.UtcNow.AddDays(-1),
                    Price = 1500.00M,
                    PaymentToDriver = 700.00M,
                    CarNumber = "К123СР77",
                    TraillerNumber = "TR5678",
                    RouteName = "TEST ROUTE NAME",
                    DriverId = 2,
                    PaymentMethodId = 2,
                    CustomerId = 2,
                    RouteId = 2,
                    StateOrderId = 2
                },
                new Transportation
                {
                    TransportationId = 3,
                    DateLoading = DateTime.UtcNow.AddDays(-2),
                    Price = 0M,
                    PaymentToDriver = 0M,
                    RouteName = "TEST ROUTE NAME",
                    CustomerId = 1,
                    RouteId =  1,
                    StateOrderId = 2
                }
            };
        }
    }

}
