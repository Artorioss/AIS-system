using System.Collections.ObjectModel;
using Test.TestData;
using WpfAppMVVM.Model.EfCode.Entities;

internal class TransportCompanyData: ITestData
{
    public ICollection<IEntity> Entities { get; set; }

    public TransportCompanyData()
    {
        Entities = new List<IEntity>()
        {
            new TransportCompany()
            {
                TransportCompanyId = 1,
                Name = "TransCo",
                SoftDeleted = false
            },
            new TransportCompany()
            {
                TransportCompanyId = 2,
                Name = "Logistics Inc.",
                SoftDeleted = false
            }
        };
    }
}
