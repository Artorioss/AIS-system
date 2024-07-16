using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.CustomComponents.Tables
{
    internal abstract class EntityTable: IObserver
    {
        protected TransportationEntities _context;
        DataGridTemplateColumn dataGridColumnDelete;
        public ObservableCollection<DataGridColumn> ColumnCollection { get; private set; }
        public ObservableCollection<IEntity> ItemsSource { get; set; }
        private List<object> deletedItems;
        public bool changedExist { get; set; }
        public IEntity SelectedItem { get; set; }
        public AsyncCommand DoubleClick { get; set; }

        public delegate Task AsyncFunction();
        public AsyncFunction asyncFunction;

        public EntityTable() 
        {
            ColumnCollection = new ObservableCollection<DataGridColumn>();
            ItemsSource = new ObservableCollection<IEntity>();
            deletedItems = new List<object>();
            changedExist = true;
            createEntityTable();
            addDeleteColumn();
            _context = (Application.Current as App)._context;
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
                ItemsSource.Add(item as IEntity);
            }
        }

        public void AddItem(IEntity Entity) 
        {
            ItemsSource.Add(Entity);
        }

        public void InsertItem(IEntity OldEntity, IEntity Entity) 
        {
            int indexOldEntity = ItemsSource.IndexOf(OldEntity);
            ItemsSource.Remove(OldEntity);
            ItemsSource.Insert(indexOldEntity, Entity);
        }

        public int GetIndexOfItem(IEntity item) 
        {
            return ItemsSource.IndexOf(item);
        }

        public void Update()
        {
            changedExist = true;
        }

        private async void deleteItem(object sender, EventArgs e)
        {
            await loadingDependentEntities();
            if (CanDelete()) 
            {
                await asyncFunction.Invoke();
                deletedItems.Add(SelectedItem);
                ItemsSource.Remove(SelectedItem);
            }
        }

        private bool CanDelete() 
        {
            MessageBoxResult result = MessageBox.Show(createMessageBoxText(), createMessageBoxCaption(), createMessageBoxButton(), MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }

        protected abstract void createEntityTable();
        protected abstract string createMessageBoxText();
        protected abstract string createMessageBoxCaption();
        protected abstract Task loadingDependentEntities();
        protected abstract MessageBoxButton createMessageBoxButton();
    }
}
