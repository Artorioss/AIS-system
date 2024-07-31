using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class CustomerData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public CustomerData()
    {
        Entities = new List<IEntity>()
        {
            new Customer()
            {
                CustomerId = 1,
                Name = "Customer One",
                SoftDeleted = false
            },
            new Customer()
            {
                CustomerId = 2,
                Name = "Customer Two",
                SoftDeleted = false
            }
        };
    }
}
