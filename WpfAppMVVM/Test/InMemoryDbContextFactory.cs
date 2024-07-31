using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Test.TestData;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using Xunit.Sdk;

namespace Test
{
    static public class InMemoryDbContextFactory
    {
        static private Dictionary<Type, ITestData> dict = new Dictionary<Type, ITestData>();
        static private TransportationEntities _context;
        static object _token = new object();

        public static TransportationEntities GetDbContext()
        {
            lock (_token) 
            {
                if (_context is null) _context = createContext();        
            }
            
            return _context;
        }

        private static TransportationEntities createContext() 
        {
            var options = new DbContextOptionsBuilder<TransportationEntities>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;
            var context = new TransportationEntities(options);
            context.Database.EnsureCreated();
            LoadAllData(context);
            return context;
        }

        private static void LoadAllData(TransportationEntities context) 
        {
            context.AddRange(getDataByType(typeof(CarBrandData)));
            context.AddRange(getDataByType(typeof(PaymentMethodData)));
            context.AddRange(getDataByType(typeof(RoutePointData)));
            context.AddRange(getDataByType(typeof(StateOrderData)));
            context.AddRange(getDataByType(typeof(TraillerBrandData)));
            context.AddRange(getDataByType(typeof(TransportCompanyData)));
            context.AddRange(getDataByType(typeof(TraillerData)));
            context.AddRange(getDataByType(typeof(StateFilterData)));
            context.AddRange(getDataByType(typeof(RouteData)));
            context.AddRange(getDataByType(typeof(DriverData)));
            context.AddRange(getDataByType(typeof(CustomerData)));
            context.AddRange(getDataByType(typeof(CarData)));
            context.AddRange(getDataByType(typeof(TransportationData)));

            context.SaveChanges();
            context.Drivers.First().Cars.Add(getDataByType(typeof(CarData)).First() as Car);
            context.Drivers.Last().Cars.Add(getDataByType(typeof(CarData)).Last() as Car);
            context.SaveChanges();
        }

        private static ICollection<IEntity> getDataByType(Type typeData) 
        {
            if (!dict.ContainsKey(typeData)) dict[typeData] = Activator.CreateInstance(typeData) as ITestData;
            return dict[typeData].Entities;
        }
    }
}
