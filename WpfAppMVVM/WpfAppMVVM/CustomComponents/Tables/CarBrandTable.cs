using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xaml;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class CarBrandTable : EntityTable
    {
        public CarBrandTable(TransportationEntities Context) : base(Context)
        {
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name") { Mode = BindingMode.TwoWay };
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnRussianBrandName = new DataGridTextColumn();
            columnRussianBrandName.Header = "Русское название";
            columnRussianBrandName.Binding = new Binding("RussianName") { Mode = BindingMode.TwoWay };
            columnRussianBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnRussianBrandName);
        }

        protected override string createMessageBoxText() 
        {
            CarBrand carBrand = SelectedItem as CarBrand;
            string messageBoxText = string.Empty;
            if (carBrand.Cars.Count > 0)
            {
                messageBoxText += "Нельзя удалить данный бренд, так как на него ссылаются из других таблиц. За данным брендом закреплены следующие автомобили:\r\n";
                int i = 0;
                foreach (Car car in carBrand.Cars)
                {
                    if (i == 5) break; 
                    messageBoxText += $"{car.Number} - {car.Brand.Name};\r\n";
                    i++;
                }
                int remainder = carBrand.Cars.Count - 5;
                if (remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других автомобилей.";
                messageBoxText += "\r\nЧтобы удалить данный бренд, разрешите зависимости.";
            }
            else messageBoxText = $"Бренд - '{(SelectedItem as CarBrand).Name}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption() 
        {
            CarBrand carBrand = SelectedItem as CarBrand;
            string caption;
            if (carBrand.Cars.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton() 
        {
            CarBrand carBrand = SelectedItem as CarBrand;
            MessageBoxButton button;
            if (carBrand.Cars.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            CarBrand carBrand = SelectedItem as CarBrand;
            var collection = _context.Entry(carBrand).Collection(cb => cb.Cars);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
