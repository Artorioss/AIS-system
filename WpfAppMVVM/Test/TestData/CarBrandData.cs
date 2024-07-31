using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class CarBrandData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public CarBrandData()
    {
        Entities = new List<IEntity>()
        {
            new CarBrand()
            {
                CarBrandId = 1,
                Name = "Mercedes-Benz",
                RussianName = "Мерседес-Бенз",
                SoftDeleted = false
            },
            new CarBrand()
            {
                CarBrandId = 2,
                Name = "BMW",
                RussianName = "БМВ",
                SoftDeleted = false
            }
        };
    }
}
