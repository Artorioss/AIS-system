using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class CarTable : EntityTable
    {
        protected override bool OnDeleteItem()
        {
            MessageBoxResult result = MessageBox.Show($"Транспорт - '{(SelectedItem as Car).Number}' будет удален.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Номер";
            columnName.Binding = new Binding("Number") { Mode = BindingMode.TwoWay };
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnBrand = new DataGridTextColumn();
            columnBrand.Header = "Бренд";
            columnBrand.Binding = new Binding("Brand.Name") { Mode = BindingMode.TwoWay };
            columnBrand.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnBrand);
        }
    }
}
