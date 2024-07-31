using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class TraillerData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public TraillerData()
    {
        Entities = new List<IEntity>()
        {
            new Trailler()
            {
                Number = "TR1234",
                BrandId = 1,
                SoftDeleted = false
            },
            new Trailler()
            {
                Number = "TR5678",
                BrandId = 2,
                SoftDeleted = false
            }
        };
    }
}
