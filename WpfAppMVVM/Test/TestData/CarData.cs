using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Migrations;
using WpfAppMVVM.Model.EfCode.Entities;

namespace Test.TestData
{
    internal class CarData: ITestData
    {
        public ICollection<IEntity> Entities { get; set; }

        public CarData()
        {
            Entities = new List<IEntity>()
            {
                new Car()
                {
                    Number = "П148АВ77",
                    BrandId = 1
                },
                new Car()
                {
                    Number = "К123СР77",
                    BrandId = 2
                }
            };
        }
    }
}
