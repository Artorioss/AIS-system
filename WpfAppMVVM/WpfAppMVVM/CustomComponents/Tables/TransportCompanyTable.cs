using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class TransportCompanyTable : EntityTable
    {
        protected override void createEntityTable()
        {
            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование";
            columnBrandName.Binding = new Binding("Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
        }

        protected override bool OnDeleteItem()
        {
            throw new NotImplementedException();
        }
    }
}
