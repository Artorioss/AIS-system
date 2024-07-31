using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class RoutePointData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public RoutePointData()
    {
        Entities = new List<IEntity>()
        {
            new RoutePoint()
            {
                RoutePointId = 1,
                Name = "Сертолово",
                SoftDeleted = false,
            },
            new RoutePoint()
            {
                RoutePointId = 2,
                Name = "г. СПБ и ЛО",
                SoftDeleted = false,
            },
            new RoutePoint()
            {
                RoutePointId = 3,
                Name = "г. Москва",
                SoftDeleted = false,
            },
            new RoutePoint()
            {
                RoutePointId = 4,
                Name = "г. Магнитогорск",
                SoftDeleted = false,
            },
            new RoutePoint()
            {
                RoutePointId = 5,
                Name = "Металлострой",
                SoftDeleted = false,
            }
        };
    }
}
