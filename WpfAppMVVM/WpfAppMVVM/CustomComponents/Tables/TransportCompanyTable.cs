using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode.Entities;

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

        protected override string createMessageBoxText()
        {
            TransportCompany transportCompany = SelectedItem as TransportCompany;
            string messageBoxText = string.Empty;
            if (transportCompany.Drivers.Count > 0)
            {
                messageBoxText += "Нельзя удалить данную компанию, так как на нее ссылаются из других таблиц. За данной компанией закреплены следующие водители:\r\n";
                int i = 0;
                foreach (Driver driver in transportCompany.Drivers)
                {
                    if (i == 5) break;
                    messageBoxText += $"{driver.Name};\r\n";
                    i++;
                }
                int remainder = transportCompany.Drivers.Count - 5;
                if (remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других водителя.";
                messageBoxText += "\r\nЧтобы удалить данного водителя, разрешите зависимости.";
            }
            else messageBoxText = $"Водитель - '{(SelectedItem as TransportCompany).Name}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            TransportCompany transportCompany = SelectedItem as TransportCompany;
            string caption;
            if (transportCompany.Drivers.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            TransportCompany transportCompany = SelectedItem as TransportCompany;
            MessageBoxButton button;
            if (transportCompany.Drivers.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            TransportCompany transportCompany = SelectedItem as TransportCompany;
            var collection = _context.Entry(transportCompany).Collection(t => t.Drivers);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
