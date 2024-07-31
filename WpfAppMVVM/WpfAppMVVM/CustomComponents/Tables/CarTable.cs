using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class CarTable : EntityTable
    {
        public CarTable(TransportationEntities Context) : base(Context)
        {
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

        protected override string createMessageBoxText()
        {
            Car car = SelectedItem as Car;
            string messageBoxText = string.Empty;
            if (car.Transportations.Count > 0)
            {
                messageBoxText += "Нельзя удалить данный транпсторт, так как на него ссылаются из других таблиц. За данным транспортом закреплены следующие заявки:\r\n";
                int i = 0;
                foreach (Transportation transportation in car.Transportations)
                {
                    if (i == 3) break;
                    messageBoxText += $"{transportation.RouteName};\r\n";
                    i++;
                }
                int remainder = car.Transportations.Count - 3;
                if (remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других заявок.";
                messageBoxText += "\r\nЧтобы удалить данный транспорт, разрешите зависимости.";
            }
            else messageBoxText = $"Транспрот - '{(SelectedItem as Car).Number}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            Car car = SelectedItem as Car;
            string caption;
            if (car.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            Car car = SelectedItem as Car;
            MessageBoxButton button;
            if (car.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            Car car = SelectedItem as Car;
            var collection = _context.Entry(car).Collection(c => c.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
