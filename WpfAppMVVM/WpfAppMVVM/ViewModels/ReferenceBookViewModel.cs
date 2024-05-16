using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;
using WpfAppMVVM.ViewModels.OtherViewModels;
using WpfAppMVVM.Views.OtherViews;
using static WpfAppMVVM.Model.CustomComboBox;

namespace WpfAppMVVM.ViewModels
{
    internal class ReferenceBookViewModel : BaseViewModel
    {
        public DataGrid dataGrid { get; set; }
        public ObservableCollection<DataGridColumn> ColumnCollection { get; private set; }
        public ObservableCollection<object> ItemsSource { get; set; }
        public DelegateCommand GetCarsData { get; private set; }
        public DelegateCommand GetBrandsData { get; private set; }
        public DelegateCommand GetCustomersData { get; private set; }
        public DelegateCommand GetDriversData { get; private set; }
        public DelegateCommand GetRoutePointsData { get; private set; }
        public DelegateCommand GetRoutesData { get; private set; }
        public DelegateCommand GetStateOrdersData { get; private set; }
        public DelegateCommand GetTraillersData { get; private set; }
        public DelegateCommand GetTransportCompaniesData { get; private set; }
        public DelegateCommand AddData { get; private set; }
        public DelegateCommand EditData { get; private set; }
        public DelegateCommand SaveChanges { get; private set; }
        public DelegateCommand DataUpdated { get; private set; }

        public delegate void ShowWindowFunction();
        private ShowWindowFunction ShowWindow;
        private TransportationEntities _context;
        private Type _currentTypeEntity;
        private bool _existСhanges = false;
        private IQueryable bufQuery;
        private List<object> deletedItems;
        private DataGridTemplateColumn DataGridColumnDelete;
        private Dictionary<Type, Action<object>> editingWindows;
        public ReferenceBookViewModel() 
        {
            _context = (Application.Current as App)._context;
            ColumnCollection = new ObservableCollection<DataGridColumn>();
            ItemsSource = new ObservableCollection<object>();

            GetCarsData = new DelegateCommand((obj) => getCarsData());
            GetBrandsData = new DelegateCommand((obj) => getBrandsData());
            GetCustomersData = new DelegateCommand((obj) => getCustomersData());
            GetDriversData = new DelegateCommand((obj) => getDriversData());
            GetRoutePointsData = new DelegateCommand((obj) => getRoutePointsData());
            GetRoutesData = new DelegateCommand((obj) => getRoutesData());
            GetStateOrdersData = new DelegateCommand((obj) => getStateOrdersData());
            GetTraillersData = new DelegateCommand((obj) => getTraillersData());
            GetTransportCompaniesData = new DelegateCommand((obj) => getTransportCompaniesData());
            DataUpdated = new DelegateCommand((obj) => dataUpdated());
            SaveChanges = new DelegateCommand((obj) => saveChangesBeforeClosing());
            AddData = new DelegateCommand((obj) => addData());
            EditData = new DelegateCommand((obj) => editData());

            editingWindows = new Dictionary<Type, Action<object>>
            {
                {typeof(Car), showCarWindow },
                {typeof(Driver), showDriverWindow},
                {typeof(Trailler), showTraillerWindow}
            };

            deletedItems = new List<object>();
            createDataGridTemplateColumn();
            getCarsData();
        }

        private void createDataGridTemplateColumn() 
        {
            DataGridColumnDelete = new DataGridTemplateColumn();
            DataGridColumnDelete.Header = "Действие";
            DataGridColumnDelete.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonFactory.SetValue(Button.ContentProperty, "Удалить");
            buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(deleteItem));

            // Установка шаблона для кнопки
            DataTemplate buttonTemplate = new DataTemplate();
            buttonTemplate.VisualTree = buttonFactory;
            DataGridColumnDelete.CellTemplate = buttonTemplate;
        }

        private void dataUpdated() 
        {
            _existСhanges = true;
        }

        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set 
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        private object _selectedItem;
        public object SelectedItem 
        {
            get => _selectedItem;
            set 
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void LoadDataInCollection(IQueryable query) 
        {
            ItemsSource.Clear();
            foreach (var items in query)
            {
                ItemsSource.Add(items);
            }
            _currentTypeEntity = query.GetType().GenericTypeArguments[0];
            bufQuery = query;
        }

        private void LoadDataInCollection()
        {
            ItemsSource.Clear();
            foreach (var items in bufQuery)
            {
                ItemsSource.Add(items);
            }
        }

        private void saveChangesBeforeClosing() 
        {
            saveChanges();
        }

        private void saveChanges()  
        {
            if (_existСhanges) 
            {
                try
                {
                    _context.SaveChanges();
                }
                catch(Exception e)
                {
                    MessageBox.Show($"Ошибка при попытке создать запись: {e.Message}", "Ошибка!",MessageBoxButton.OK, MessageBoxImage.Error);
                }
                _existСhanges = false;
            } 
        }

        private void showCarWindow() 
        {
            CreateCarViewModel createCarViewModel = new CreateCarViewModel();
            CreateCarWindow createCarWindow = new CreateCarWindow();
            createCarWindow.DataContext = createCarViewModel;
            createCarWindow.ShowDialog();
        }

        private void showCarWindow(object selectedItem)
        {
            CreateCarViewModel createCarViewModel = new CreateCarViewModel(selectedItem as Car);
            CreateCarWindow createCarWindow = new CreateCarWindow();
            createCarWindow.DataContext = createCarViewModel;
            createCarWindow.ShowDialog();
        }

        private void showDriverWindow() 
        {
            CreateDriverViewModel createDriverViewModel = new CreateDriverViewModel();
            CreateDriverWindow createDriverWindow = new CreateDriverWindow();
            createDriverWindow.DataContext = createDriverViewModel;
            createDriverWindow.ShowDialog();
        }

        private void showDriverWindow(object selectedItem)
        {
            CreateDriverViewModel createDriverViewModel = new CreateDriverViewModel(selectedItem as Driver);
            CreateDriverWindow createDriverWindow = new CreateDriverWindow();
            createDriverWindow.DataContext = createDriverViewModel;
            createDriverWindow.ShowDialog();
        }

        private void showTraillerWindow() 
        {
            CreateTraillerViewModel createTraillerViewModel = new CreateTraillerViewModel();
            CreateTraillerWindow createTraillerWindow = new CreateTraillerWindow();
            createTraillerWindow.DataContext = createTraillerViewModel;
            createTraillerWindow.ShowDialog();
        }

        private void showTraillerWindow(object selectedItem)
        {
            CreateTraillerViewModel createTraillerViewModel = new CreateTraillerViewModel(selectedItem as Trailler);
            CreateTraillerWindow createTraillerWindow = new CreateTraillerWindow();
            createTraillerWindow.DataContext = createTraillerViewModel;
            createTraillerWindow.ShowDialog();
        }

        private void deleteItem(object sender, EventArgs e) 
        {
            _context.Remove(SelectedItem);
            deletedItems.Add(SelectedItem);
            ItemsSource.Remove(SelectedItem);
            _existСhanges = true;
        }

        private void getCarsData() 
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showCarWindow;
            ColumnCollection.Clear();
            var data = _context.Cars.Include(car => car.Brand);

            DataGridTextColumn columnCarBrand = new DataGridTextColumn();
            columnCarBrand.Header = "Бренд";
            columnCarBrand.Binding = new Binding("Brand.Name");
            columnCarBrand.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridCheckBoxColumn columnIsTrack = new DataGridCheckBoxColumn();
            columnIsTrack.Header = "Тягач";
            columnIsTrack.Binding = new Binding("IsTruck");
            columnIsTrack.Width = new DataGridLength(50, DataGridLengthUnitType.Auto);

            ColumnCollection.Add(columnCarBrand);
            ColumnCollection.Add(columnNumber);
            ColumnCollection.Add(columnIsTrack);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(data);
        }

        private void getBrandsData()
        {
            saveChanges();
            IsReadOnly = false;
            ShowWindow = null;
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name") { Mode = BindingMode.TwoWay };
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnRussianBrandName = new DataGridTextColumn();
            columnRussianBrandName.Header = "Русское название";
            columnRussianBrandName.Binding = new Binding("RussianBrandName") { Mode = BindingMode.TwoWay };
            columnRussianBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnRussianBrandName);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.Brands;
            LoadDataInCollection(data);
        }

        private void getCustomersData()
        {
            saveChanges();
            IsReadOnly = false;
            ShowWindow = null;
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.Customers;
            LoadDataInCollection(data);
        }

        private void getDriversData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showDriverWindow;
            ColumnCollection.Clear();

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
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.Drivers.Include(driver => driver.TransportCompany);
            LoadDataInCollection(data);
        }

        private void getRoutePointsData()
        {
            saveChanges();
            IsReadOnly = false;
            ShowWindow = null;
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.RoutePoints;
            LoadDataInCollection(data);
        }

        private void getRoutesData()
        {
            saveChanges();
            IsReadOnly = false;
            ShowWindow = null;
            ColumnCollection.Clear();

            DataGridTextColumn columnRouteName = new DataGridTextColumn();
            columnRouteName.Header = "Маршрут";
            columnRouteName.Binding = new Binding("RouteName");
            columnRouteName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnRouteName);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.Routes;
            LoadDataInCollection(data);
        }

        private void getStateOrdersData()
        {
            saveChanges();
            IsReadOnly = false;
            ShowWindow = null;
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.StateOrders;
            LoadDataInCollection(data);
        }

        private void getTraillersData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showTraillerWindow;
            ColumnCollection.Clear();

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Бренд";
            columnBrandName.Binding = new Binding("Brand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
            ColumnCollection.Add(columnNumber);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.Traillers.Include(trailer => trailer.Brand);
            LoadDataInCollection(data);
        }

        private void getTransportCompaniesData()
        {
            saveChanges();
            IsReadOnly = false;
            ShowWindow = null;
            ColumnCollection.Clear();

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование";
            columnBrandName.Binding = new Binding("Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
            ColumnCollection.Add(DataGridColumnDelete);

            var data = _context.TransportCompanies;
            LoadDataInCollection(data);
        }

        private void addData() 
        {
            if (ShowWindow != null) 
            {
                ShowWindow();
                LoadDataInCollection();
            } 
            else 
            {
                object entity = Activator.CreateInstance(_currentTypeEntity);
                _context.Add(entity);
                ItemsSource.Add(entity);
                _existСhanges = true;
            }
        }

        private void editData()
        {
            if (ShowWindow != null)
            {
                editingWindows[_currentTypeEntity].Invoke(SelectedItem);
                LoadDataInCollection();
            }
        }
    }
}
