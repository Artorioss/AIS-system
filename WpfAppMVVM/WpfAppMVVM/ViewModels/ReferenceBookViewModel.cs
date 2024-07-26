using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Threading;
using WpfAppMVVM.CustomComponents.Tables;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.OtherViewModels;

namespace WpfAppMVVM.ViewModels
{
    internal class ReferenceBookViewModel : NotifyService, IObservable
    {
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
        public AsyncCommand GetFiltresDataAsync { get; private set; }
        public AsyncCommand AddData { get; private set; }
        public AsyncCommand GetDataByValue { get; private set; }
        public AsyncCommand GetNextPageAsync { get; private set; }
        public AsyncCommand GetPreviosPageAsync { get; private set; }

        private TransportationEntities _context;
        private PaginationService _paginationService;
        private BaseViewModel _viewModel;
        private Type _typeVM;
        private Dictionary<Type, EntityTable> _entityTablesDict;
        private DispatcherTimer _loadingTimer;
        private EntityTable _entityTable;
        private List<IObserver> _observers;
        public bool ChangedExists { get; private set; } = false;

        public EntityTable EntityTable
        {
            get => _entityTable;
            private set
            {
                _entityTable = value;
                OnPropertyChanged(nameof(EntityTable));
            }
        }

        public ReferenceBookViewModel() 
        {
            _context = (Application.Current as App)._context;
            _paginationService = new PaginationService();
            _entityTablesDict = new Dictionary<Type, EntityTable>();
            _observers = new List<IObserver>();

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
            GetFiltresDataAsync = new AsyncCommand(async (obj) => await getFilterData());
            AddData = new AsyncCommand(async (obj) => await addData());
            //GetDataByValue = new AsyncCommand(getDataByValue);
            GetNextPageAsync = new AsyncCommand(async (obj) => await getNextPage());
            GetPreviosPageAsync = new AsyncCommand(async (obj) => await getPreviosPage());

            _loadingTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            _loadingTimer.Tick += LoadingTimer_Tick;

            getCarsData();
        }

        public IEntity SelectedItem
        {
            get => EntityTable.SelectedItem;
            set
            {
                EntityTable.SelectedItem = value;
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
                loadDataInTableByValue(value);
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

        string _text;
        public string CountPages 
        {
            get => _text;
            set 
            {
                _text = value;
                OnPropertyChanged(nameof(CountPages));
            }
        }

        private void clearSearchValueIfExist() 
        {
            _searchingValue = null;
            OnPropertyChanged(nameof(SearchingValue));
        }

        private async Task getNextPage() 
        {
            _loadingTimer.Start();
            EntityTable.LoadDataInCollection(await _paginationService.GetNextPageAsync());
            _loadingTimer.Stop();
            notifyElements();
        }

        private async Task getPreviosPage() 
        {

            _loadingTimer.Start();
            EntityTable.LoadDataInCollection(await _paginationService.GetPreviosPageAsync());
            _loadingTimer.Stop();
            notifyElements();
        }

        private void notifyElements() 
        {
            CountPages = _paginationService.CountPages > 0 ? $"Страница {_paginationService.CurrentPage}\\{_paginationService.CountPages}" : "Данные не найдены";
            OnPropertyChanged(nameof(CanGetNextPage));
            OnPropertyChanged(nameof(CanGetPreviosPage));
        }

        private async Task saveChangesAsync()  
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                MessageBox.Show($"Ошибка при попытке удалить запись: {e.Message}", "Ошибка!",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            if (CountPages == "Загрузка данных .") CountPages = "Загрузка данных ..";
            else if (CountPages == "Загрузка данных ..") CountPages = "Загрузка данных ...";
            else CountPages = "Загрузка данных .";
        }

        private EntityTable getEntityTable(Type typeEntityTable)
        {
            if (!_entityTablesDict.ContainsKey(typeEntityTable)) _entityTablesDict[typeEntityTable] = createEntityTable(typeEntityTable);
            return _entityTablesDict[typeEntityTable];
        }

        private EntityTable createEntityTable(Type type) 
        {
            EntityTable entityTable = Activator.CreateInstance(type) as EntityTable;
            entityTable.DoubleClick = new AsyncCommand(async (obj) => await editData());
            entityTable.asyncFunction += deleteItem;
            RegisterObserver(entityTable);
            return entityTable;
        }

        private async Task LoadDataInTable() 
        {
            _loadingTimer.Start();
            EntityTable.LoadDataInCollection(await _paginationService.GetCurrentPageAsync());
            EntityTable.changedExist = false;
            _loadingTimer.Stop();
        }

        private async Task loadDataInTableByValue(object obj)
        {
            string text = obj as string;
            _loadingTimer.Start();
            EntityTable.LoadDataInCollection(await _paginationService.GetDataByValueAsync(text));
            EntityTable.changedExist = true;
            _loadingTimer.Stop();
            notifyElements();
        }

        private async Task getCarsData() 
        {
            clearSearchValueIfExist();
            _typeVM = typeof(CarViewModel);

            EntityTable = getEntityTable(typeof(CarTable));
            _paginationService.SetQuery(_context.Cars.Include(car => car.Brand));
            _paginationService.SetSearchFunction(obj => _context.Cars.Include(car => car.Brand)
                                    .Where(b => b.Number.ToLower().Contains(obj.ToLower())
                                             || b.Brand.Name.ToLower().Contains(obj.ToLower())
                                             || b.Brand.RussianName.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getTraillerBrandsData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(TraillerBrandViewModel);
            
            EntityTable = getEntityTable(typeof(TraillerBrandTable));
            _paginationService.SetQuery(_context.TraillerBrands);
            _paginationService.SetSearchFunction(obj => _context.TraillerBrands.Where(b => b.Name.ToLower().Contains(obj.ToLower())
                                             || b.RussianName.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getCarBrandsDataAsync()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(CarBrandViewModel);
            EntityTable = getEntityTable(typeof(CarBrandTable));
            _paginationService.SetQuery(_context.CarBrands);
            _paginationService.SetSearchFunction(obj => _context.CarBrands.Where(b => b.Name.ToLower().Contains(obj.ToLower())
                                             || b.RussianName.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getCustomersData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(CustomerViewModel);
            EntityTable = getEntityTable(typeof(CustomerTable));
            _paginationService.SetQuery(_context.Customers);
            _paginationService.SetSearchFunction(obj => _context.Customers.Where(b => b.Name.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getDriversData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(DriverViewModel);
            EntityTable = getEntityTable(typeof(DriverTable));
            _paginationService.SetQuery(_context.Drivers.Include(driver => driver.TransportCompany));
            _paginationService.SetSearchFunction(obj => _context.Drivers.Include(d => d.TransportCompany)
                                                                        .Where(b => b.Name.ToLower().Contains(obj.ToLower())
                                                                                 || b.TransportCompany.Name.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getRoutePointsData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(RoutePointViewModel);
            EntityTable = getEntityTable(typeof(RoutePointTable));
            _paginationService.SetQuery(_context.RoutePoints);
            _paginationService.SetSearchFunction(obj => _context.RoutePoints.Where(rp => rp.Name.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getRoutesData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(RouteViewModel);
            EntityTable = getEntityTable(typeof(RouteTable));
            _paginationService.SetQuery(_context.Routes.Include(r => r.Transportations));
            _paginationService.SetSearchFunction(obj => _context.Routes.Where(b => b.RouteName.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getStateOrdersData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(StateOrderViewModel);
            EntityTable = getEntityTable(typeof(StateOrderTable));
            _paginationService.SetQuery(_context.StateOrders);
            _paginationService.SetSearchFunction(obj => _context.StateOrders.Where(s => s.Name.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getTraillersData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(TraillerViewModel);
            EntityTable = getEntityTable(typeof(TraillerTable));
            _paginationService.SetQuery(_context.Traillers.Include(trailer => trailer.Brand));
            _paginationService.SetSearchFunction(obj => _context.Traillers.Include(t => t.Brand)
                                         .Where(b => b.Number.ToLower().Contains(obj.ToLower())
                                                  || b.Brand.Name.ToLower().Contains(obj.ToLower())
                                                  || b.Brand.RussianName.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getTransportCompaniesData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(TransportCompanyViewModel);
            EntityTable = getEntityTable(typeof(TransportCompanyTable));
            _paginationService.SetQuery(_context.TransportCompanies);
            _paginationService.SetSearchFunction(obj => _context.TransportCompanies.Where(b => b.Name.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task getFilterData()
        {
            clearSearchValueIfExist();
            _typeVM = typeof(FilterStateViewModel);
            EntityTable = getEntityTable(typeof(FilterTable)); 
            _paginationService.SetQuery(_context.StateFilter);
            _paginationService.SetSearchFunction(obj => _context.StateFilter.Where(f => f.Name.ToLower().Contains(obj.ToLower())));
            if (EntityTable.changedExist) await LoadDataInTable();
            notifyElements();
        }

        private async Task addData() 
        {
            _viewModel = Activator.CreateInstance(_typeVM) as BaseViewModel;
            await _viewModel.ShowDialog();
            if (_viewModel.changedExist) _entityTable.AddItem(await _viewModel.GetEntity());
            NotifyObservers();
        }

        private async Task editData()
        {
            _viewModel = Activator.CreateInstance(_typeVM, EntityTable.SelectedItem) as BaseViewModel;
            await _viewModel.ShowDialog();
            if (_viewModel.changedExist) 
            {
                _entityTable.InsertItem(SelectedItem, await _viewModel.GetEntity());
                ChangedExists = true;
            } 
            NotifyObservers();
        }

        private async Task deleteItem()
        {
            SelectedItem.SoftDeleted = true;
            await saveChangesAsync();
            if (EntityTable.ItemsSource.Count == 1) await getPreviosPage();
            
        }

        public void RegisterObserver(IObserver observer) => _observers.Add(observer);
        public void UnregisterObserver(IObserver observer) => _observers.Remove(observer);
        public void NotifyObservers() => _observers.ForEach(o => o.Update());
    }
}
