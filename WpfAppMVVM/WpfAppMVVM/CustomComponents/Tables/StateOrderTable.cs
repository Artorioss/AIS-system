using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class StateOrderTable : EntityTable
    {
        public StateOrderTable(TransportationEntities Context) : base(Context)
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
            StateOrder state = SelectedItem as StateOrder;
            string messageBoxText = string.Empty;
            if (state.Transportations.Count > 0)
            {
                messageBoxText += $"Нельзя удалить данное состояние, так как на нее ссылается {state.Transportations.Count} заявок.";
                messageBoxText += "\r\nЧтобы удалить данное состояние, разрешите зависимости.";
            }
            else messageBoxText = $"Состояние - '{(SelectedItem as StateOrder).Name}' будет удалено. Продолжить?";
            return messageBoxText;
        }

        protected override string createMessageBoxCaption()
        {
            StateOrder state = SelectedItem as StateOrder;
            string caption;
            if (state.Transportations.Count > 0) caption = "Невозможно выполнить действие";
            else caption = "Вы уверены?";
            return caption;
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            StateOrder state = SelectedItem as StateOrder;
            MessageBoxButton button;
            if (state.Transportations.Count > 0) button = MessageBoxButton.OK;
            else button = MessageBoxButton.YesNo;
            return button;
        }

        protected override async Task loadingDependentEntities()
        {
            StateOrder state = SelectedItem as StateOrder;
            var collection = _context.Entry(state).Collection(s => s.Transportations);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
