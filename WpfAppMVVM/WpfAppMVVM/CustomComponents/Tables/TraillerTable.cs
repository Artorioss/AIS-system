using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class TraillerTable : EntityTable
    {
        protected override void createEntityTable()
        {
            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Бренд";
            columnBrandName.Binding = new Binding("Brand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
            ColumnCollection.Add(columnNumber);
        }

        protected override bool OnDeleteItem()
        {
            throw new NotImplementedException();
        }
    }
}
