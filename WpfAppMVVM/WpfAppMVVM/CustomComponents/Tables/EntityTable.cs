using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfAppMVVM.Model.Command;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal abstract class EntityTable
    {
        DataGridTemplateColumn dataGridColumnDelete;
        public ObservableCollection<DataGridColumn> ColumnCollection { get; private set; }
        public ObservableCollection<ICloneable> ItemsSource { get; set; }
        private List<object> deletedItems;
        public ICloneable SelectedItem { get; set; }
        public AsyncCommand DoubleClick { get; set; }

        public event EventHandler? OnDelete;

        public EntityTable() 
        {
            ColumnCollection = new ObservableCollection<DataGridColumn>();
            ItemsSource = new ObservableCollection<ICloneable>();
            deletedItems = new List<object>();

            createEntityTable();
            addDeleteColumn();
        }

        private void addDeleteColumn()
        {
            dataGridColumnDelete = new DataGridTemplateColumn();
            dataGridColumnDelete.Header = "Действие";
            dataGridColumnDelete.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonFactory.SetValue(Button.ContentProperty, "Удалить");
            buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(deleteItem));

            DataTemplate buttonTemplate = new DataTemplate();
            buttonTemplate.VisualTree = buttonFactory;
            dataGridColumnDelete.CellTemplate = buttonTemplate;

            ColumnCollection.Add(dataGridColumnDelete);
        }

        public void LoadDataInCollection(IEnumerable items)
        {
            ItemsSource.Clear();
            foreach (var item in items)
            {
                ItemsSource.Add(item as ICloneable);
            }
        }

        private void deleteItem(object sender, EventArgs e)
        {
            if (OnDeleteItem()) 
            {
                OnDelete?.Invoke(sender, e);
                deletedItems.Add(SelectedItem);
                ItemsSource.Remove(SelectedItem);
            }
        }

        protected abstract bool OnDeleteItem();
        protected abstract void createEntityTable();
    }
}
