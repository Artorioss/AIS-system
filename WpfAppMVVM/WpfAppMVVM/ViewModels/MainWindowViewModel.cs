using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.QueryObjects;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels
{
    internal class MainWindowViewModel : NotifyService
    {
        public TransportationEntities _transportationEntities { get; set; }
        public AsyncCommand CreateTransportationAsync { get; private set; }
        public AsyncCommand ShowReferencesBookAsync { get; private set; }
        public AsyncCommand EditDataAsync { get; private set; }
        public AsyncCommand DeleteCommandAsync { get; set; }
        public DelegateCommand GetItemsByFilter { get; set; }
        public DelegateCommand CopyCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }
        public AsyncCommand CloseRequestAsyncCommand { get; set; }

        private DispatcherTimer _loadingTimer;
        private MonthService _monthService;
        public ObservableCollection<TransportationDTO> ItemsSource { get; set; }

        private List<StateFilter> _stateFilters;
        public List<StateFilter> StateFilters
        {
            get => _stateFilters;
            set
            {
                _stateFilters = value;
                OnPropertyChanged(nameof(StateFilters));
            }
        }

        private List<StateOrder> _states;
        public List<StateOrder> States
        {
            get => _states;
            set
            {
                _states = value;
                OnPropertyChanged(nameof(States));
            }
        }

        public Visibility ContextMenuVisible
        {
            get => TransportationDTO != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public List<StateOrder> StateOrdersForContextMenu
        {
            get => States.Where(st => st.StateOrderId != TransportationDTO.State).ToList();
        }

        private string _stateText = "Данные не найдены";
        public string StateText
        {
            get => _stateText;
            set
            {
                _stateText = value;
                OnPropertyChanged(nameof(StateText));
            }
        }

        public bool DateFilterIsEnabled
        {
            get => Years == null ? false : Years.Count > 0;
        }

        private StateFilter _selectedFilter;
        public StateFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (value != null)
                {
                    _selectedFilter = value;
                    OnPropertyChanged(nameof(SelectedFilter));
                    updateDateFiltres();
                }
            }
        }

        private List<int> _years;
        public List<int> Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }

        private List<string> _months;
        public List<string> Months
        {
            get => _months;
            set
            {
                _months = value;
                OnPropertyChanged(nameof(Months));
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                setMonthsByYear(value);
            }
        }

        private async Task setMonthsByYear(int year)
        {
            _months = _monthService.GetMonths(await getMonthsByYearFromDbAsync(year));
            setSelectedMonth();
            OnPropertyChanged(nameof(Months));
        }

        private async Task<List<int>> getMonthsByYearFromDbAsync(int year)
        {
            return await _context.Transportations
                    .Where(t => t.DateLoading.Value.Year == year && t.StateOrder.StateFilters.Any(sf => sf.StateFilterId == SelectedFilter.StateFilterId))
                    .Select(t => t.DateLoading.Value.Month)
                    .Distinct()
                    .ToListAsync();
        }

        private int _selectedMonth;
        public string SelectedMonth
        {
            get => _monthService.GetMonth(_selectedMonth);
            set
            {
                _selectedMonth = _monthService.GetMonth(value);
                OnPropertyChanged(nameof(SelectedMonth));
                loadItemsInItemSource();
            }
        }

        private TransportationDTO _transportation;
        public TransportationDTO TransportationDTO
        {
            get => _transportation;
            set
            {
                _transportation = value;
                OnPropertyChanged(nameof(TransportationDTO));
                OnPropertyChanged(nameof(ContextMenuVisible));
            }
        }

        private void setDate(DateTime dateTime)
        {
            if (!Months.Contains(_monthService.GetMonth(dateTime.Month))) Months.Add(_monthService.GetMonth(dateTime.Month));
            _selectedMonth = dateTime.Month;
            OnPropertyChanged(nameof(SelectedMonth));
            if (!Years.Contains(dateTime.Year)) Years.Add(dateTime.Year);
            _selectedYear = dateTime.Year;
            OnPropertyChanged(nameof(SelectedYear));
        }

        private void setNewDate(DateTime dateTime)
        {
            _years = new List<int> { dateTime.Year };
            SelectedYear = _years.First();
            OnPropertyChanged(nameof(Years));
            _months = new List<string> { _monthService.GetMonth(dateTime.Month) };
            SelectedMonth = _months.First();
            OnPropertyChanged(nameof(Months));
        }

        private async Task setNewStatusAsync(object obj) 
        {
            var selectedStateOrderId = (obj as StateOrder).StateOrderId;
            var transportation = await _context.Transportations.FindAsync(TransportationDTO.TransportationId);
            transportation.StateOrderId = selectedStateOrderId;
            if (SelectedFilter.StateOrders.FirstOrDefault(st => st.StateOrderId == selectedStateOrderId) is null)
            {
                ItemsSource.Remove(TransportationDTO);
                TransportationDTO = null;
            }
            await _context.SaveChangesAsync();
        }

        public MainWindowViewModel()
        {
            _transportationEntities = (Application.Current as App)._context;
            ItemsSource = new ObservableCollection<TransportationDTO>();

            _loadingTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            _loadingTimer.Tick += LoadingTimer_Tick;

            _monthService = new MonthService();

            States = _context.StateOrders.ToList();
            StateFilters = _context.StateFilter.Include(sf => sf.StateOrders).ToList();
            SelectedFilter = StateFilters.FirstOrDefault();

            CreateTransportationAsync = new AsyncCommand(async (obj) => await showTransportationWindow());
            ShowReferencesBookAsync = new AsyncCommand(async (obj) => await showWindowReferencesBook());
            EditDataAsync = new AsyncCommand(async (obj) => await showTransportationForEditWindow());
            DeleteCommandAsync = new AsyncCommand(async (obj) => await onDelete());
            CopyCommand = new DelegateCommand((obj) => copy());
            SortCommand = new DelegateCommand((obj) => onSorting());
            CloseRequestAsyncCommand = new AsyncCommand(setNewStatusAsync);
        }

        private async Task updateDateFiltres() 
        {
            await setYears();
            if (Years.Count > 0) setSelectedYear();
            else 
            {
                Months.Clear();
                Months.Add(string.Empty);
                _selectedMonth = 0;
                OnPropertyChanged(nameof(SelectedMonth));
                ItemsSource.Clear();
                StateText = "Данные не найдены";
            }
            OnPropertyChanged(nameof(DateFilterIsEnabled));
        }

        private void LoadingTimer_Tick(object sender, EventArgs e) 
        {
            if (StateText.Contains("Найдено") || StateText == "Загрузка данных ...") StateText = "Загрузка данных .";
            else if (StateText == "Загрузка данных .") StateText = "Загрузка данных ..";
            else if (StateText == "Загрузка данных ..") StateText = "Загрузка данных ...";
        }

        private async Task setYears() 
        {
            Years = await _context.Transportations
                            .Where(t => t.StateOrder.StateFilters.Any(sf => sf.StateFilterId == SelectedFilter.StateFilterId))
                            .Select(t => t.DateLoading.Value.Date.Year)
                            .Distinct()
                            .ToListAsync();
        }
       

        private void setSelectedYear() 
        {
            if (Years.Contains(DateTime.Now.Year)) SelectedYear = DateTime.Now.Year;
            else if(Years.Count > 0) SelectedYear = Years.Last();
        }

        private void setSelectedMonth() 
        {
            if (Months.Contains(_monthService.GetMonth(DateTime.Now.Month))) SelectedMonth = _monthService.GetMonth(DateTime.Now.Month);
            else if(Months.Count > 0) SelectedMonth = Months.FirstOrDefault();
        }

        private async Task loadItemsInItemSource() 
        {
            _loadingTimer.Start();
            loadData(await getItems());
            _loadingTimer.Stop();
            StateText = $"Найдено {ItemsSource.Count} записей";
        }

        private async Task<List<TransportationDTO>> getItems() 
        {
            var startDate = new DateTime(SelectedYear, _selectedMonth, 1);
            var endDate = startDate.AddMonths(1);
            return await _transportationEntities.Transportations
                                                .Include(t => t.Driver)
                                                .Include(t => t.Customer)
                                                .Include(t => t.StateOrder)
                                                .Where(t => t.DateLoading.HasValue &&
                                                t.DateLoading.Value >= startDate &&
                                                t.DateLoading.Value < endDate &&
                                                t.StateOrder.StateFilters.Any(sf => sf.StateFilterId == SelectedFilter.StateFilterId))
                                    .TransportationToDTO()
                                    .ToListAsync();
        }

        private void loadData(List<TransportationDTO> list) 
        {
            ItemsSource.Clear();
            foreach (var item in list) ItemsSource.Add(item);
        }

        private async Task showTransportationWindow()
        {
            CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel();
            await creatingTransportationViewModel.ShowDialog();

            if (creatingTransportationViewModel.changedExist)
            {
                var transportation = creatingTransportationViewModel.Transportation;
                if (transportation.DateLoading.Value.Month != _selectedMonth || transportation.DateLoading.Value.Year != SelectedYear)
                {
                    DateTime date = transportation.DateLoading.Value;
                    try
                    {
                        setDate(date);
                        await loadItemsInItemSource();
                    }
                    catch (NullReferenceException) 
                    {
                        setNewDate(date);
                        OnPropertyChanged(nameof(DateFilterIsEnabled));
                    }
                }
                else ItemsSource.Add(createTransportationDTO(transportation)); // TODO проверить, возможны ошибки
            }
        }

        private TransportationDTO createTransportationDTO(Transportation transportation) 
        {
            TransportationDTO transportationDTO = new TransportationDTO()
            {
                TransportationId = transportation.TransportationId,
                DateLoading = transportation.DateLoading.Value.Date.ToString(),
                CustomerName = transportation.Customer.Name,
                DriverName = transportation.Driver.Name,
                TransportCompanyName = transportation.Driver.TransportCompany.Name,
                Price = transportation.Price,
                PaymentToDriver = transportation.PaymentToDriver,
                Delta = transportation.Price - transportation.PaymentToDriver,
                RouteName = transportation.RouteName,
                State = transportation.StateOrder.StateOrderId
            };

            return transportationDTO;
        }

        private async Task showTransportationForEditWindow()
        {
            if (TransportationDTO != null) 
            {
                _loadingTimer.Start();
                CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel(await getTransportationById(TransportationDTO.TransportationId));
                _loadingTimer.Stop();
                StateText = "Редактирование заявки";
                await creatingTransportationViewModel.ShowDialog();

                if (creatingTransportationViewModel.changedExist)
                {
                    var transportation = creatingTransportationViewModel.Transportation;
                    if (transportation.DateLoading.Value.Month != _selectedMonth || transportation.DateLoading.Value.Year != SelectedYear)
                    {
                        DateTime date = transportation.DateLoading.Value;
                        setDate(date);
                        await loadItemsInItemSource();
                        TransportationDTO = null;
                    }
                    else 
                    {
                        var transportationDTO = createTransportationDTO(transportation);
                        ItemsSource.Insert(ItemsSource.IndexOf(TransportationDTO), transportationDTO);
                        ItemsSource.Remove(TransportationDTO);
                        TransportationDTO = transportationDTO;
                    }
                }
                StateText = $"Найдено {ItemsSource.Count} записей";
            }
        }

        private async Task<Transportation> getTransportationById(int id) 
        {
            var transportation = _context.Transportations.Local.SingleOrDefault(t => t.TransportationId == id);
            if (transportation is null) transportation = await getTransportationFromDbAsync(id);
            return transportation;
        }

        private async Task<Transportation> getTransportationFromDbAsync(int id) 
        {
            Transportation transportation = await _context.Transportations.Include(t => t.Car)
                                                 .ThenInclude(c => c.Brand)
                                                 .Include(t => t.Trailler)
                                                 .ThenInclude(t => t.Brand)
                                                 .Include(t => t.Route)
                                                 .Include(t => t.Customer)
                                                 .Include(t => t.Driver)
                                                 .ThenInclude(d => d.TransportCompany)
                                                 .Include(t => t.StateOrder)
                                                 .Include(t => t.PaymentMethod)
                         .SingleAsync(t => t.TransportationId == id);
            return transportation;
        }

        private async Task showWindowReferencesBook()
        {
            ReferenceBookViewModel referenceBookViewModel = new ReferenceBookViewModel();
            StateText = $"В отделе 'Справочники'";
            await (Application.Current as App).DisplayRootRegistry.ShowModalPresentation(referenceBookViewModel);
            if (referenceBookViewModel.ChangedExists) 
            {
                if (Years.Count != 0) await loadItemsInItemSource();
                else await updateDateFiltres();
            } 
            else StateText = $"Найдено {ItemsSource.Count} записей";
        }

        private async Task onDelete() 
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{TransportationDTO.RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) await delete();          
        }

        private async Task delete() 
        {
            var transportation = await _context.Transportations.FindAsync(TransportationDTO.TransportationId);
            _context.Transportations.Remove(transportation);
            await _context.SaveChangesAsync();
            ItemsSource.Remove(TransportationDTO);
        }

        private void copy() 
        {
            Clipboard.SetText(TransportationDTO.RouteName);
        }

        private void onSorting() 
        {
            TransportationDTO = null;
        }
    }
}
