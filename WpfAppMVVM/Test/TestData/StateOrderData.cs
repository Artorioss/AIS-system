using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class StateOrderData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public StateOrderData()
    {
        Entities = new List<IEntity>()
        {
            new StateOrder()
            {
                StateOrderId = 1,
                Name = "Закрыта",
                SoftDeleted = false
            },
            new StateOrder()
            {
                StateOrderId = 2,
                Name = "В обработке",
                SoftDeleted = false
            },
            new StateOrder()
            {
                StateOrderId = 3,
                Name = "Срыв",
                SoftDeleted = false
            }
        };
    }
}
