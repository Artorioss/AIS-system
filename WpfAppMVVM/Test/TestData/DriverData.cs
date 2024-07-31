using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.EfCode.Entities;

namespace Test.TestData
{
    internal class DriverData: ITestData
    {
        public ICollection<IEntity> Entities { get; set; }
        public DriverData()
        {
            Entities = new List<IEntity>()
            {
                new Driver()
                {
                    DriverId = 1,
                    Name = "John Doe",
                    TransportCompanyId = 1
                },
                new Driver()
                {
                    DriverId = 2,
                    Name = "Jane Smith",
                    TransportCompanyId = 2
                }
            };
        }
    }
}
