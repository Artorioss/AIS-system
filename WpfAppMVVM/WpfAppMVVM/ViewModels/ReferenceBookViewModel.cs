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
using System.Windows.Threading;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.OtherViewModels;
using WpfAppMVVM.Views.OtherViews;

namespace WpfAppMVVM.ViewModels
{
    internal class ReferenceBookViewModel : BaseViewModel
    {
        public DataGrid dataGrid { get; set; }
        public ObservableCollection<DataGridColumn> ColumnCollection { get; private set; }
        public ObservableCollection<object> ItemsSource { get; set; }
        public AsyncCommand GetCarsDataAsync { get; private set; }
        public AsyncCommand GetCarBrandsDataAsync { get; private set; }
        public AsyncCommand GetTraillerBrandsDataAsync { get; private set; }
        public AsyncCommand GetCustomersDataAsync { get; private set; }
        public AsyncCommand GetDriversDataAsync { get; private set; }
        public AsyncCommand GetRoutePointsDataAsync { get; private set; }
        public AsyncCommand GetRoutesDataAsync { get; private set; }
        public AsyncCommand GetStateOrdersDataAsync { get; private set; }
        public AsyncCommand GetTraillersDataAsync { get; private set; }
        public AsyncCommand GetTransportCompaniesDataAsync { get; private set; }
        public DelegateCommand AddData { get; private set; }
        public DelegateCommand EditData { get; private set; }
        public DelegateCommand SaveChanges { get; private set; }
        public DelegateCommand DataUpdated { get; private set; }
        public AsyncCommand GetDataByValue { get; private set; } 
        public AsyncCommand GetNextPageAsync { get; private set; }
        public AsyncCommand GetPreviosPageAsync { get; private set; }

        public delegate void ShowWindowFunction();
        private ShowWindowFunction ShowWindow;
        private TransportationEntities _context;
        private Type _currentTypeEntity;
        private bool _existСhanges = false;
        private IQueryable bufQuery;
        private List<object> deletedItems;
        private DataGridTemplateColumn DataGridColumnDelete;
        private Dictionary<Type, Action<object>> editingWindows;
        private PaginationService _paginationService;

        public ReferenceBookViewModel() 
        {
            _context = (Application.Current as App)._context;
            ColumnCollection = new ObservableCollection<DataGridColumn>();
            ItemsSource = new ObservableCollection<object>();
            _paginationService = new PaginationService();

            GetCarsDataAsync = new AsyncCommand((obj) => getCarsData());
            GetCarBrandsDataAsync = new AsyncCommand(async (obj) => await getCarBrandsDataAsync());
            GetTraillerBrandsDataAsync = new AsyncCommand(async (obj) => await getTraillerBrandsData());
            GetCustomersDataAsync = new AsyncCommand(async (obj) => await getCustomersData());
            GetDriversDataAsync = new AsyncCommand(async (obj) => await getDriversData());
            GetRoutePointsDataAsync = new AsyncCommand(async (obj) => await getRoutePointsData());
            GetRoutesDataAsync = new AsyncCommand(async (obj) => await getRoutesData());
            GetStateOrdersDataAsync = new AsyncCommand(async (obj) => await getStateOrdersData());
            GetTraillersDataAsync = new AsyncCommand(async (obj) => await getTraillersData());
            GetTransportCompaniesDataAsync = new AsyncCommand(async (obj) => await getTransportCompaniesData());
            DataUpdated = new DelegateCommand((obj) => dataUpdated());
            SaveChanges = new DelegateCommand((obj) => saveChangesBeforeClosing());
            AddData = new DelegateCommand((obj) => addData());
            EditData = new DelegateCommand((obj) => editData());
            GetDataByValue = new AsyncCommand(getDataByValue);
            GetNextPageAsync = new AsyncCommand(async (obj) => await getNextPage());
            GetPreviosPageAsync = new AsyncCommand(async (obj) => await getPreviosPage());
            

            editingWindows = new Dictionary<Type, Action<object>>
            {
                {typeof(Car), showCarWindow },
                {typeof(Driver), showDriverWindow},
                {typeof(Trailler), showTraillerWindow},
                {typeof(CarBrand), showCarBrandWindow },
                {typeof(TraillerBrand), showTraillerBrandWindow },
                {typeof(Customer), showCustomerWindow },
                {typeof(RoutePoint), showRoutePointWindow},
                {typeof(Route), showRouteWindow },
                {typeof(StateOrder), showStateOrderWindow},
                {typeof(TransportCompany), showTransportCompanyWindow}
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

        private string _searchingValue;
        public string SearchingValue 
        {
            get => _searchingValue;
            set 
            {
                _searchingValue = value;
                OnPropertyChanged(nameof(SearchingValue));
            }
        }

        public bool CanGetNextPage 
        {
            get => _paginationService.CanGetNext;
        }

        public bool CanGetPreviosPage 
        {
            get => _paginationService.CanGetPrevios;
        }

        public string CountPages 
        {
            get => _paginationService.CountPages > 0 ? $"Страница {_paginationService.CurrentPage}\\{_paginationService.CountPages}" : "Данные не найдены";
        } 

        private async Task getNextPage() 
        {
            LoadDataInCollection(await _paginationService.GetNextPageAsync());
            notifyElements();
        }

        private async Task getPreviosPage() 
        {
            LoadDataInCollection(await _paginationService.GetPreviosPageAsync());
            notifyElements();
        }

        private void notifyElements() 
        {
            OnPropertyChanged(nameof(CountPages));
            OnPropertyChanged(nameof(CanGetNextPage));
            OnPropertyChanged(nameof(CanGetPreviosPage));
        }

        private void LoadDataInCollection(IEnumerable query) 
        {
            ItemsSource.Clear();
            foreach (var items in query)
            {
                ItemsSource.Add(items);
            }
            _currentTypeEntity = query.GetType().GenericTypeArguments[0];
        }

        private void LoadDataInCollection()
        {
            ItemsSource.Clear();
            foreach (var items in _paginationService.GetCurrentPageAsync().Result)
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
            CarViewModel carViewModel = new CarViewModel();
            CarWindow carWindow = new CarWindow();
            carWindow.DataContext = carViewModel;
            carWindow.ShowDialog();
        }

        private void showCarWindow(object selectedItem)
        {
            CarViewModel carViewModel = new CarViewModel(selectedItem as Car);
            CarWindow carWindow = new CarWindow();
            carWindow.DataContext = carViewModel;
            carWindow.ShowDialog();
        }

        private void showDriverWindow()
        {
            DriverViewModel driverViewModel = new DriverViewModel();
            DriverWindow driverWindow = new DriverWindow();
            driverWindow.DataContext = driverViewModel;
            driverWindow.ShowDialog();
        }

        private void showDriverWindow(object selectedItem)
        {
            DriverViewModel driverViewModel = new DriverViewModel(selectedItem as Driver);
            DriverWindow driverWindow = new DriverWindow();
            driverWindow.DataContext = driverViewModel;
            driverWindow.ShowDialog();
        }

        private void showTraillerWindow()
        {
            TraillerViewModel traillerViewModel = new TraillerViewModel();
            TraillerWindow traillerWindow = new TraillerWindow();
            traillerWindow.DataContext = traillerViewModel;
            traillerWindow.ShowDialog();
        }

        private void showTraillerWindow(object selectedItem)
        {
            TraillerViewModel traillerViewModel = new TraillerViewModel(selectedItem as Trailler);
            TraillerWindow traillerWindow = new TraillerWindow();
            traillerWindow.DataContext = traillerViewModel;
            traillerWindow.ShowDialog();
        }

        private void showTraillerBrandWindow()
        {
            TraillerBrandViewModel traillerViewModel = new TraillerBrandViewModel();
            TraillerBrandWindow traillerWindow = new TraillerBrandWindow();
            traillerWindow.DataContext = traillerViewModel;
            traillerWindow.ShowDialog();
        }

        private void showTraillerBrandWindow(object selectedItem)
        {
            TraillerBrandViewModel traillerViewModel = new TraillerBrandViewModel(selectedItem as TraillerBrand);
            TraillerBrandWindow traillerWindow = new TraillerBrandWindow();
            traillerWindow.DataContext = traillerViewModel;
            traillerWindow.ShowDialog();
        }

        private void showCarBrandWindow() 
        {
            CarBrandViewModel carBrandViewModel = new CarBrandViewModel();
            CarBrandWindow brandWindow = new CarBrandWindow();
            brandWindow.DataContext = carBrandViewModel;
            brandWindow.ShowDialog();
        }

        private void showCarBrandWindow(object selectedItem)
        {
            CarBrandViewModel carBrandViewModel = new CarBrandViewModel(selectedItem as CarBrand);
            CarBrandWindow brandWindow = new CarBrandWindow();
            brandWindow.DataContext = carBrandViewModel;
            brandWindow.ShowDialog();
        }

        private void showCustomerWindow() 
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.DataContext = customerViewModel;
            customerWindow.ShowDialog();
        }

        private void showCustomerWindow(object selectedItem) 
        {
            CustomerViewModel customerViewModel = new CustomerViewModel(selectedItem as Customer);
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.DataContext = customerViewModel;
            customerWindow.ShowDialog();
        }

        private void showRoutePointWindow()
        {
            RoutePointViewModel routePointViewModel = new RoutePointViewModel();
            RoutePointWindow routePointWindow = new RoutePointWindow();
            routePointWindow.DataContext = routePointViewModel;
            routePointWindow.ShowDialog();
        }

        private void showRoutePointWindow(object selectedItem)
        {
            RoutePointViewModel routePointViewModel = new RoutePointViewModel(selectedItem as RoutePoint);
            RoutePointWindow routePointWindow = new RoutePointWindow();
            routePointWindow.DataContext = routePointViewModel;
            routePointWindow.ShowDialog();
        }

        private void showRouteWindow()
        {
            RouteViewModel routeViewModel = new RouteViewModel();
            RouteWindow routeWindow = new RouteWindow();
            routeWindow.DataContext = routeViewModel;
            routeWindow.ShowDialog();
        }

        private void showRouteWindow(object selectedItem)
        {
            RouteViewModel routeViewModel = new RouteViewModel(selectedItem as Route);
            RouteWindow routeWindow = new RouteWindow();
            routeWindow.DataContext = routeViewModel;
            routeWindow.ShowDialog();
        }

        private void showStateOrderWindow()
        {
            StateOrderViewModel stateOrderViewModel = new StateOrderViewModel();
            StateWindow stateWindow = new StateWindow();
            stateWindow.DataContext = stateOrderViewModel;
            stateWindow.ShowDialog();
        }

        private void showStateOrderWindow(object selectedItem)
        {
            StateOrderViewModel stateOrderViewModel = new StateOrderViewModel(selectedItem as StateOrder);
            StateWindow stateWindow = new StateWindow();
            stateWindow.DataContext = stateOrderViewModel;
            stateWindow.ShowDialog();
        }

        private void showTransportCompanyWindow()
        {
            TransportCompanyViewModel transportCompanyViewModel = new TransportCompanyViewModel();
            TransportCompanyWindow transportCompanyWindow = new TransportCompanyWindow();
            transportCompanyWindow.DataContext = transportCompanyViewModel;
            transportCompanyWindow.ShowDialog();
        }

        private void showTransportCompanyWindow(object selectedItem)
        {
            TransportCompanyViewModel transportCompanyViewModel = new TransportCompanyViewModel(selectedItem as TransportCompany);
            TransportCompanyWindow transportCompanyWindow = new TransportCompanyWindow();
            transportCompanyWindow.DataContext = transportCompanyViewModel;
            transportCompanyWindow.ShowDialog();
        }


        private void deleteItem(object sender, EventArgs e) 
        {
            _context.Remove(SelectedItem);
            deletedItems.Add(SelectedItem);
            ItemsSource.Remove(SelectedItem);
            _existСhanges = true;
        }

        private async Task getCarsData() 
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showCarWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.Cars.Include(car => car.Brand));
            _paginationService.SetSearchFunction(obj => _context.Cars.Include(car => car.Brand)
                                    .Where(b => b.Number.ToLower().Contains(obj.ToLower())
                                             || b.Brand.Name.ToLower().Contains(obj.ToLower())
                                             || b.Brand.RussianName.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnCarBrand = new DataGridTextColumn();
            columnCarBrand.Header = "Бренд";
            columnCarBrand.Binding = new Binding("Brand.Name");
            columnCarBrand.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnCarBrand);
            ColumnCollection.Add(columnNumber);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getTraillerBrandsData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showTraillerBrandWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.TraillerBrands);
            _paginationService.SetSearchFunction(obj => _context.TraillerBrands.Where(b => b.Name.ToLower().Contains(obj.ToLower())
                                             || b.RussianName.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name") { Mode = BindingMode.TwoWay };
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnRussianBrandName = new DataGridTextColumn();
            columnRussianBrandName.Header = "Русское название";
            columnRussianBrandName.Binding = new Binding("RussianName") { Mode = BindingMode.TwoWay };
            columnRussianBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnRussianBrandName);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getCarBrandsDataAsync()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showCarBrandWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.CarBrands);
            _paginationService.SetSearchFunction(obj => _context.CarBrands.Where(b => b.Name.ToLower().Contains(obj.ToLower())
                                             || b.RussianName.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name") { Mode = BindingMode.TwoWay };
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnRussianBrandName = new DataGridTextColumn();
            columnRussianBrandName.Header = "Русское название";
            columnRussianBrandName.Binding = new Binding("RussianName") { Mode = BindingMode.TwoWay };
            columnRussianBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnRussianBrandName);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getCustomersData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showCustomerWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.Customers);
            _paginationService.SetSearchFunction(obj => _context.Customers.Where(b => b.Name.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getDriversData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showDriverWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.Drivers.Include(driver => driver.TransportCompany));
            _paginationService.SetSearchFunction(obj => _context.Drivers.Include(d => d.TransportCompany)
                                                                        .Where(b => b.Name.ToLower().Contains(obj.ToLower())
                                                                                 || b.TransportCompany.Name.ToLower().Contains(obj.ToLower())));

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

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getRoutePointsData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showRoutePointWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.RoutePoints);
            _paginationService.SetSearchFunction(obj => _context.RoutePoints.Where(rp => rp.Name.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getRoutesData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showRouteWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.Routes.Include(r => r.Transportations));
            _paginationService.SetSearchFunction(obj => _context.Routes.Where(b => b.RouteName.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnRouteName = new DataGridTextColumn();
            columnRouteName.Header = "Маршрут";
            columnRouteName.Binding = new Binding("RouteName");
            columnRouteName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnCount = new DataGridTextColumn();
            columnCount.Header = "Кол-во перевозок";
            columnCount.Binding = new Binding("Transportations.Count");
            columnCount.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            ColumnCollection.Add(columnRouteName);
            ColumnCollection.Add(columnCount);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getStateOrdersData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showStateOrderWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.StateOrders);
            _paginationService.SetSearchFunction(obj => _context.StateOrders.Where(s => s.Name.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getTraillersData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showTraillerWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.Traillers.Include(trailer => trailer.Brand));
            _paginationService.SetSearchFunction(obj => _context.Traillers.Include(t => t.Brand)
                                         .Where(b => b.Number.ToLower().Contains(obj.ToLower())
                                                  || b.Brand.Name.ToLower().Contains(obj.ToLower())
                                                  || b.Brand.RussianName.ToLower().Contains(obj.ToLower())));

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

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
        }

        private async Task getTransportCompaniesData()
        {
            saveChanges();
            IsReadOnly = true;
            ShowWindow = showTransportCompanyWindow;
            ColumnCollection.Clear();
            _paginationService.SetQuery(_context.TransportCompanies);
            _paginationService.SetSearchFunction(obj => _context.TransportCompanies.Where(b => b.Name.ToLower().Contains(obj.ToLower())));

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование";
            columnBrandName.Binding = new Binding("Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
            ColumnCollection.Add(DataGridColumnDelete);

            LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            notifyElements();
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
                editingWindows[_currentTypeEntity]?.Invoke(SelectedItem);
                LoadDataInCollection();
            }
        }

        //Поиск
        private async Task getDataByValue(object obj) 
        {
            string text = obj as string;
            LoadDataInCollection(await _paginationService.GetDataByValueAsync(text));
            OnPropertyChanged(nameof(CountPages));
            OnPropertyChanged(nameof(CanGetNextPage));
            OnPropertyChanged(nameof(CanGetPreviosPage));
        }
    }
}
