using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class StateFilterData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public StateFilterData()
    {
        Entities = new List<IEntity>()
        {
            new StateFilter()
            {
                StateFilterId = 1,
                Name = "Закрыты",
                SoftDeleted = false,
            },
            new StateFilter()
            {
                StateFilterId = 2,
                Name = "Срывы",
                SoftDeleted = false,
            },
            new StateFilter()
            {
                StateFilterId = 3,
                Name = "В обработке",
                SoftDeleted = false,
            },
            new StateFilter()
            {
                StateFilterId = 4,
                Name = "Все типы",
                SoftDeleted = false,
            }
        };
    }
}
