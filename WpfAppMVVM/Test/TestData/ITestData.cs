using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.EfCode.Entities;

namespace Test.TestData
{
    public interface ITestData
    {
        ICollection<IEntity> Entities { get; set; }
    }

}
