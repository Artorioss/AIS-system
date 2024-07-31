using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class RouteData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }
    public RouteData()
    {
        Entities = new List<IEntity>()
        {
            new Route()
            {
                RouteId = 1,
                RouteName = "Сертолово - г. СПБ и ЛО",
                SoftDeleted = false
            },
            new Route()
            {
                RouteId = 2,
                RouteName = "г. Москва, г. Магнитогорск - Металлострой",
                SoftDeleted = false
            }
        };
    }
}
