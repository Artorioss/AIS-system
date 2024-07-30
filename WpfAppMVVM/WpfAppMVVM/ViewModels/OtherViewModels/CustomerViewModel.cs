using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using System.Collections.ObjectModel;
using System.Windows;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CustomerViewModel : BaseViewModel
    {
        Customer _customer;
        MonthService _monthService;
        public ObservableCollection<Transportation> Transportations { get; set; }
        public AsyncCommand DeleteAsyncCommand { get; set; }
        public AsyncCommand ShowWindowAsyncCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }

        public CustomerViewModel()
        {
            _customer = new Customer();
            mode = Mode.Additing;
            settingUp();
        }

        public CustomerViewModel(Customer customer)
        {
            mode = Mode.Editing;
            _customer = customer;
            settingUp();
            WindowName = "Редактирование клиента";
        }

        protected override void cloneEntity()
        {
            _customer = _customer.Clone() as Customer;
        }

        protected override async Task loadReferenceData()
        {
            setYears();
            setSelectedYear();
        }

        private void settingUp()
        {
            _monthService = new MonthService();
            Years = new List<int>();
            Months = new List<string>();
            Transportations = new ObservableCollection<Transportation>();
        }

        private string _windowName = "Создание клиента";
        public string WindowName 
        {
            get => _windowName;
            set 
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public string CustomerName 
        {
            get => _customer.Name;
            set 
            {
                _customer.Name = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        public int CountTransportations
        {
            get => Transportations.Count;
        }

        private Transportation _transportation;
        public Transportation SelectedTransportation 
        {
            get => _transportation;
            set 
            {
                _transportation = value;
                OnPropertyChanged(nameof(SelectedTransportation));
            }
        }

        public bool GroupElementsIsEnabled 
        {
            get => _customer.Transportations.Count > 0;
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
            _months = _monthService.GetMonths(getMonthByYear(year));
            setSelectedMonth();
            OnPropertyChanged(nameof(Months));
        }

        private List<int> getMonthByYear(int year) 
        {
            return _customer.Transportations
                            .Where(t => t.DateLoading.Value.Year == year)
                            .Select(t => t.DateLoading.Value.Month)
                            .Distinct()
                            .ToList();
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

        private void setYears()
        {
            Years = _customer.Transportations
                    .Select(t => t.DateLoading.Value.Date.Year)
                    .Distinct()
                    .ToList();
        }

        private void setSelectedYear()
        {
            if (Years.Contains(DateTime.Now.Year)) SelectedYear = DateTime.Now.Year;
            else if (Years.Count != 0) SelectedYear = Years.Last();
            
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

            var list = _customer.Transportations
                       .Where(t => t.DateLoading.HasValue &&
                            t.DateLoading.Value >= startDate &&
                            t.DateLoading.Value < endDate);

            loadData(list);
            OnPropertyChanged(nameof(CountTransportations));
        }

        private void onSort()
        {
            SelectedTransportation = null;
        }

        private void loadData(IEnumerable<Transportation> list)
        {
            Transportations.Clear();
            foreach (var item in list) Transportations.Add(item);
        }

        private async Task deleteEntityAsync(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{(obj as Transportation).RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) await delete(obj as Transportation);
        }

        private async Task delete(Transportation dto)
        {
            (await _context.Transportations.SingleAsync(t => t.TransportationId == dto.TransportationId)).SoftDeleted = true;
            await SaveChangesAsync();
            Transportations.Remove(dto);
        }


        protected override async Task<bool> dataIsCorrect()
        {
            if (string.IsNullOrEmpty(_customer.Name)) 
            {
                MessageBox.Show("Неверно указано наименование компании заказчика.","Неправильно заполнены данные.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override void setCommands()
        {
            DeleteAsyncCommand = new AsyncCommand(deleteEntityAsync);
            ShowWindowAsyncCommand = new AsyncCommand(async (obj) => await showWindowForEdit());
            SortCommand = new DelegateCommand((obj) => onSort());
        }

        private async Task showWindowForEdit() 
        {
            if (SelectedTransportation != null) 
            {
                CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel(SelectedTransportation);

                if (creatingTransportationViewModel.changedExist)
                {
                    var transportation = creatingTransportationViewModel.Transportation;
                    if (transportation.DateLoading.Value.Month != _selectedMonth || transportation.DateLoading.Value.Year != SelectedYear)
                    {
                        DateTime date = transportation.DateLoading.Value;
                        setDate(date);
                        getItems();
                    }
                    else 
                    {
                        var newTransportation = await creatingTransportationViewModel.GetEntity() as Transportation;
                        int id = Transportations.IndexOf(SelectedTransportation);
                        Transportations.Remove(SelectedTransportation);
                        Transportations.Insert(id, newTransportation);
                        SelectedTransportation = newTransportation;
                    }
                }
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

        protected override async Task addEntity()
        {
            var customer = await _context.Customers.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == _customer.Name && c.SoftDeleted);
            if (customer != null) 
            {
                _customer.CustomerId = customer.CustomerId;
                customer.SetFields(_customer);
            }
            else await _context.AddAsync(_customer);
        }

        protected override async Task updateEntity()
        {
            var cm = await _context.Customers.FindAsync(_customer.CustomerId);
            cm.SetFields(_customer);
        }

        public override async Task<IEntity> GetEntity() => await _context.Customers.FindAsync(_customer.CustomerId);
    }
}
