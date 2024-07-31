using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class DriverTable : EntityTable
    {
        public DriverTable(TransportationEntities Context) : base(Context)
        {
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnTransportCompany = new DataGridTextColumn();
            columnTransportCompany.Header = "Транспортная компания";
            columnTransportCompany.Binding = new Binding("TransportCompany.Name");
            columnTransportCompany.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnTransportCompany);
        }

        protected override string createMessageBoxText()
        {
            Driver driver = SelectedItem as Driver;
            string messageBoxText = string.Empty;
            if (driver.Transportations.Count > 0)
            {
                messageBoxText += "Нельзя удалить данного водителя, так как на него ссылаются из других таблиц. За данным водителем закреплены следующие заявки:\r\n";
                int i = 0;
                foreach (Transportation transportation in driver.Transportations)
                {
                    if (i == 3) break;
                    messageBoxText += $"{transportation.RouteName};\r\n";
                    i++;
                }
                int remainder = driver.Transportations.Count - 3;
                if (remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других заявок.";
                messageBoxText += "\r\nЧтобы удалить данного водителя, разрешите зависимости.";
            }
            else messageBoxText = $"Водитель - '{(SelectedItem as Driver).Name}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            Driver driver = SelectedItem as Driver;
            string caption;
            if (driver.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            Driver driver = SelectedItem as Driver;
            MessageBoxButton button;
            if (driver.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            Driver driver = SelectedItem as Driver;
            var collection = _context.Entry(driver).Collection(d => d.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
