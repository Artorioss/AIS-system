using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal class FilterTable : EntityTable
    {
        public FilterTable(TransportationEntities Context) : base(Context)
        {
        }

        protected override void createEntityTable()
        {
            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name") { Mode = BindingMode.TwoWay };
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
        }

        protected override MessageBoxButton createMessageBoxButton()
        {
            return MessageBoxButton.YesNo;
        }

        protected override string createMessageBoxCaption()
        {
            string caption = "Вы уверены?";
            return caption;
        }

        protected override string createMessageBoxText()
        {
            StateFilter stateFilter = SelectedItem as StateFilter;
            string messageBoxText = $"Фильтр - '{stateFilter.Name}' будет удален. Продолжить?";
            return messageBoxText;
        }

        protected override async Task loadingDependentEntities()
        {
            StateFilter stateFilter = SelectedItem as StateFilter;
            var collection = _context.Entry(stateFilter).Collection(cb => cb.StateOrders);
            if (!collection.IsLoaded) await collection.LoadAsync();
        }
    }
}
