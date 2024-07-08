using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class RouteTable : EntityTable
    {
        protected override void createEntityTable()
        {
            DataGridTextColumn columnRouteName = new DataGridTextColumn();
            columnRouteName.Header = "Маршрут";
            columnRouteName.Binding = new Binding("RouteName");
            columnRouteName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnCount = new DataGridTextColumn();
            columnCount.Header = "Кол-во перевозок";
            columnCount.Binding = new Binding("Transportations.Count");
            columnCount.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            ColumnCollection.Add(columnRouteName);
            ColumnCollection.Add(columnCount);
        }

        protected override bool OnDeleteItem()
        {
            throw new NotImplementedException();
        }
    }
}
