using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class PaymentMethodData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }
    public PaymentMethodData()
    {
        Entities = new List<IEntity>()
        {
            new PaymentMethod()
            {
                PaymentMethodId = 1,
                Name = "Credit Card",
                SoftDeleted = false
            },
            new PaymentMethod()
            {
                PaymentMethodId = 2,
                Name = "Cash",
                SoftDeleted = false
            }
        };
    }
}
