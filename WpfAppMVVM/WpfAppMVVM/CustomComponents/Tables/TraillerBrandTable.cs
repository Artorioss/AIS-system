using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class TraillerBrandTable : EntityTable
    {
        public TraillerBrandTable(TransportationEntities Context) : base(Context)
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
            TraillerBrand brand = SelectedItem as TraillerBrand;
            string messageBoxText = string.Empty;
            if (brand.Traillers.Count > 0)
            {
                messageBoxText += "Нельзя удалить данный бренд, так как на него ссылаются из других таблиц. За данным брендом закреплены следующие прицепы:\r\n";
                int i = 0;
                foreach (Trailler trailler in brand.Traillers)
                {
                    if (i == 5) break;
                    messageBoxText += $"{trailler.Number} - {trailler.Brand.Name};\r\n";
                    i++;
                }
                int remainder = brand.Traillers.Count;
                if(remainder > 0) messageBoxText += $"\r\nИ ещё {remainder} других прицепов.";
                messageBoxText += "\r\nЧтобы удалить данный бренд, разрешите зависимости.";
            }
            else messageBoxText = $"Бренд - '{(SelectedItem as TraillerBrand).Name}' будет удален. Продолжить?";
            return messageBoxText;
        }
        
        protected override string createMessageBoxCaption()
        {
            TraillerBrand brand = SelectedItem as TraillerBrand;
            string caption;
            if (brand.Traillers.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            TraillerBrand brand = SelectedItem as TraillerBrand;
            MessageBoxButton button;
            if (brand.Traillers.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            TraillerBrand brand = SelectedItem as TraillerBrand;
            var collection = _context.Entry(brand).Collection(b => b.Traillers);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
