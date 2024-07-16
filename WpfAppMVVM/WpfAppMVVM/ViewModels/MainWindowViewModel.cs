using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
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
    internal class MainWindowViewModel : BaseViewModel
    {
        public TransportationEntities _transportationEntities { get; set; }
        public DelegateCommand CreateTransportation { get; private set; }
        public DelegateCommand ShowReferencesBook { get; private set; }
        public DelegateCommand EditData { get; private set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand GetItemsByFilter { get; set; }
        public DelegateCommand CopyCommand { get; set; }
        public DelegateCommand SortCommand {get; set;}

        private MonthService _monthService;
        public ObservableCollection<TransportationDTO> ItemsSource { get; set; }
        private List<StateOrder> _stateOrders;
        public List<StateOrder> StateOrders
        {
            get => _stateOrders;
            set
            {
                _stateOrders = value;
                OnPropertyChanged(nameof(StateOrders));
            }
        }

        private StateOrder _selectedState;
        public StateOrder SelectedState
        {
            get => _selectedState;
            set
            {
                _selectedState = value;
                OnPropertyChanged(nameof(SelectedState));
                getItems();
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

        private void setMonthsByYear(int year) 
        {
            _months = _monthService.GetMonths(_context.Transportations
                                                  .Where(t => t.DateLoading.Value.Year == year)
                                                  .Select(t => t.DateLoading.Value.Month)
                                                  .Distinct()
                                                  .ToList());
            setSelectedMonth();
            OnPropertyChanged(nameof(Months));
        }

        private int _selectedMonth;
        public string SelectedMonth
        {
            get => _monthService.GetMonth(_selectedMonth);
            set
            {
                _selectedMonth = _monthService.GetMonth(value);
                OnPropertyChanged(nameof(SelectedMonth));
                getItems();
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
            }
        }

        private void setDate(DateTime dateTime) 
        {
            if (!Months.Contains(_monthService.GetMonth(dateTime.Month))) Months.Add(_monthService.GetMonth(dateTime.Month));
            _selectedMonth = dateTime.Month;
            OnPropertyChanged(nameof(SelectedMonth));
            if(!Years.Contains(dateTime.Year)) Years.Add(dateTime.Year);
            _selectedYear = dateTime.Year;
            OnPropertyChanged(nameof(SelectedYear));
        }

        public MainWindowViewModel()
        {
            _transportationEntities = (Application.Current as App)._context;
            ItemsSource = new ObservableCollection<TransportationDTO>();

            StateOrders = _context.StateOrders.ToList();
            _selectedState = StateOrders.First();
            OnPropertyChanged(nameof(SelectedState));

            _monthService = new MonthService();
            setYears();
            setSelectedYear();

            CreateTransportation = new DelegateCommand((obj) => showTransportationWindow());
            ShowReferencesBook = new DelegateCommand((obj) => showWindowReferencesBook());
            EditData = new DelegateCommand((obj) => showTransportationForEditWindow());
            DeleteCommand = new DelegateCommand((obj) => onDelete());
            CopyCommand = new DelegateCommand((obj) => copy());
            SortCommand = new DelegateCommand((obj) => onSorting());
        }

        private void setYears() 
        {
            Years = _context.Transportations
                            .Select(t => t.DateLoading.Value.Date.Year)
                            .Distinct()
                            .ToList();
        }

        private void setSelectedYear() 
        {
            if (Years.Contains(DateTime.Now.Year)) SelectedYear = DateTime.Now.Year;
            else SelectedYear = Years.Last();
        }

        private void setSelectedMonth() 
        {
            if (Months.Contains(_monthService.GetMonth(DateTime.Now.Month))) SelectedMonth = _monthService.GetMonth(DateTime.Now.Month);
            else SelectedMonth = Months.FirstOrDefault();
        }

        private void getItems() 
        {
            var startDate = new DateTime(SelectedYear, _selectedMonth, 1);
            var endDate = startDate.AddMonths(1);

            var list = _transportationEntities.Transportations
                .Include(t => t.Driver)
                .Include(t => t.Customer)
                .Include(t => t.StateOrder)
                .Where(t => t.DateLoading.HasValue &&
                            t.DateLoading.Value >= startDate &&
                            t.DateLoading.Value < endDate &&
                            t.StateOrderId == SelectedState.StateOrderId)
                .TransportationToDTO();

            loadData(list);
        }

        private void loadData(IQueryable<TransportationDTO> list) 
        {
            ItemsSource.Clear();
            foreach (var item in list) ItemsSource.Add(item);
        }

        private void showTransportationWindow()
        {
            CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
            CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel();
            creatingTransportationWindow.DataContext = creatingTransportationViewModel;
            creatingTransportationWindow.ShowDialog();

            if (creatingTransportationViewModel.IsContextChanged)
            {
                if (creatingTransportationViewModel.Transportation.DateLoading.Value.Month != _selectedMonth || creatingTransportationViewModel.Transportation.DateLoading.Value.Year != SelectedYear) 
                {
                    DateTime date = creatingTransportationViewModel.Transportation.DateLoading.Value;
                    setDate(date);
                    getItems();
                }
                else ItemsSource.Add(_context.Transportations
                                             .TransportationToDTO()
                                             .Single(tr => tr.TransportationId == creatingTransportationViewModel.Transportation.TransportationId));
            }
        }

        private void showTransportationForEditWindow()
        {
            if (TransportationDTO != null) 
            {
                CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
                CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel(TransportationDTO.TransportationId);
                creatingTransportationWindow.DataContext = creatingTransportationViewModel;
                creatingTransportationWindow.ShowDialog();

                if (creatingTransportationViewModel.IsContextChanged)
                {
                    var entity = _context.Transportations.TransportationToDTO().Single(tr => tr.TransportationId == creatingTransportationViewModel.Transportation.TransportationId);
                    if (creatingTransportationViewModel.Transportation.DateLoading.Value.Month != _selectedMonth || creatingTransportationViewModel.Transportation.DateLoading.Value.Year != SelectedYear)
                    {
                        DateTime date = creatingTransportationViewModel.Transportation.DateLoading.Value;
                        setDate(date);
                        getItems();
                    }
                    else ItemsSource.Insert(ItemsSource.IndexOf(TransportationDTO), entity);
                    ItemsSource.Remove(TransportationDTO);
                    TransportationDTO = entity;
                }
            }
        }

        private void showWindowReferencesBook()
        {
            WindowReferencesBook windowReferencesBook = new WindowReferencesBook();
            windowReferencesBook.ShowDialog();
        }

        private void onDelete() 
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{TransportationDTO.RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) delete();          
        }

        private void delete() 
        {
            _context.Transportations.Remove(_context.Transportations.Single(t => t.TransportationId == TransportationDTO.TransportationId));
            _context.SaveChanges();
            ItemsSource.Remove(TransportationDTO);
            OnPropertyChanged(nameof(ItemsSource));
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
