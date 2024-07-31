using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.Model.EfCode;

namespace WpfAppMVVM.CustomComponents.Tables
{
    class PaymentMethodTable: EntityTable
    {
        public PaymentMethodTable(TransportationEntities Context) : base(Context)
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
            PaymentMethod paymentMethod = SelectedItem as PaymentMethod;
            string messageBoxText = string.Empty;
            if (paymentMethod.Transportations.Count > 0)
            {
                messageBoxText += $"Нельзя удалить данный способ оплаты, так как на него ссылаются {paymentMethod.Transportations.Count} заявок.";
                messageBoxText += "\r\nЧтобы удалить данный способ оплаты, разрешите зависимости.";
            }
            else messageBoxText = $"Состояние - '{(SelectedItem as PaymentMethod).Name}' будет удалено. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            PaymentMethod paymentMethod = SelectedItem as PaymentMethod;
            string caption;
            if (paymentMethod.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            PaymentMethod paymentMethod = SelectedItem as PaymentMethod;
            MessageBoxButton button;
            if (paymentMethod.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            PaymentMethod paymentMethod = SelectedItem as PaymentMethod;
            var collection = _context.Entry(paymentMethod).Collection(s => s.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
