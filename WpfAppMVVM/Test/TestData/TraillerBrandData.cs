using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class TraillerBrandData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public TraillerBrandData()
    {
        Entities = new List<IEntity>()
        {
            new TraillerBrand()
            {
                TraillerBrandId = 1,
                Name = "Shmitz",
                RussianName = "Шмитц",
                SoftDeleted = false
            },
            new TraillerBrand()
            {
                TraillerBrandId = 2,
                Name = "KRONE",
                RussianName = "Кроне",
                SoftDeleted = false
            }
        };
    }
}
