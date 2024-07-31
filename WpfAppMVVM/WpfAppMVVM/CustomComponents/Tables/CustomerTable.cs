using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class CustomerTable : EntityTable
    {
        public CustomerTable(TransportationEntities Context) : base(Context)
        {
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
        }

        protected override string createMessageBoxText()
        {
            Customer customer = SelectedItem as Customer;
            string messageBoxText = string.Empty;
            if (customer.Transportations.Count > 0)
            {
                messageBoxText += "Нельзя удалить данного клиента, так как на него ссылаются из других таблиц. За данным клиентом закреплены следующие заявки:\r\n";
                int i = 0;
                foreach (Transportation transportation in customer.Transportations)
                {
                    if (i == 3) break;
                    messageBoxText += $"{transportation.RouteName};\r\n";
                    i++;
                }
                int remainder = customer.Transportations.Count - i;
                if (remainder > 0) messageBoxText += $"\r\nИ еще {remainder} других заявок.";
                messageBoxText += "\r\nЧтобы удалить данного клиента, разрешите зависимости.";
            }
            else messageBoxText = $"Клиент - '{(SelectedItem as Customer).Name}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            Customer customer = SelectedItem as Customer;
            string caption;
            if (customer.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            Customer customer = SelectedItem as Customer;
            MessageBoxButton button;
            if (customer.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            Customer customer = SelectedItem as Customer;
            var collection = _context.Entry(customer).Collection(c => c.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
