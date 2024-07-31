using Microsoft.EntityFrameworkCore;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    public class StateOrderViewModel: BaseViewModel
    {
        StateOrder _stateOrder;
        MonthService _monthService;
        public ObservableCollection<Transportation> Transportations { get; set; }
        public AsyncCommand DeleteAsyncCommand { get; set; }
        public DelegateCommand ShowWindowCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }

        public StateOrderViewModel(TransportationEntities Context, IDisplayRootRegistry displayRootRegistry) : base(Context, displayRootRegistry)
        {
            _stateOrder = new StateOrder();
            mode = Mode.Additing;
            settingUp();
        }

        public StateOrderViewModel(StateOrder stateOrder, TransportationEntities Context, IDisplayRootRegistry displayRootRegistry) : base(Context, displayRootRegistry)
        {
            mode = Mode.Editing;
            _stateOrder = stateOrder;
            settingUp();
            WindowName = "Редактирование состояния";
        }

        protected override void cloneEntity()
        {
            _stateOrder = _stateOrder.Clone() as StateOrder;   
        }

        protected override async Task loadReferenceData()
        {
            await setYears();
            setSelectedYear();
        }

        private void settingUp()
        {
            Transportations = new ObservableCollection<Transportation>();
            _monthService = new MonthService();
            Years = new List<int>();
            Months = new List<string>();

        }

        private string _windowName = "Создание состояния";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public string StateOrderName
        {
            get => _stateOrder.Name;
            set
            {
                _stateOrder.Name = value;
                OnPropertyChanged(nameof(StateOrderName));
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
            get => Transportations.Count > 0;
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
            _months = _monthService.GetMonths(await _context.Transportations
                                                  .Where(t => t.DateLoading.Value.Year == year
                                                         && t.StateOrderId == _stateOrder.StateOrderId)
                                                  .Select(t => t.DateLoading.Value.Month)
                                                  .Distinct()
                                                  .ToListAsync());
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

        private async Task setYears()
        {
            Years = await _context.Transportations
                            .Where(t => t.StateOrderId == _stateOrder.StateOrderId)
                            .Select(t => t.DateLoading.Value.Date.Year)
                            .Distinct()
                            .ToListAsync();
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

            var list = _context.Transportations
                .Where(t => t.DateLoading.HasValue &&
                            t.DateLoading.Value >= startDate &&
                            t.DateLoading.Value < endDate
                            && t.StateOrderId == _stateOrder.StateOrderId);

            loadData(list);
            OnPropertyChanged(nameof(CountTransportations));
        }

        private void onSort()
        {
            SelectedTransportation = null;
        }

        private void loadData(IQueryable<Transportation> list)
        {
            Transportations.Clear();
            foreach (var item in list) Transportations.Add(item);
        }

        private async Task deleteEntity(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{(obj as Transportation).RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) await delete(obj as Transportation);
        }

        private async Task delete(Transportation dto)
        {
            var transportation = await _context.Transportations.SingleAsync(t => t.TransportationId == dto.TransportationId);
            _context.Transportations.Remove(transportation);
            await SaveChangesAsync();
            _context.SaveChanges();
            Transportations.Remove(dto);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if (string.IsNullOrEmpty(_stateOrder.Name))
            {
                MessageBox.Show("Неверно указано наименование компании заказчика.", "Неправильно заполнены данные.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override void setCommands()
        {
            DeleteAsyncCommand = new AsyncCommand(deleteEntity);
            ShowWindowCommand = new DelegateCommand((obj) => showWindowForEdit());
            SortCommand = new DelegateCommand((obj) => onSort());
        }

        private async void showWindowForEdit()
        {
            if (SelectedTransportation != null)
            {
                CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel(SelectedTransportation, _context, _rootRegistry);
                await creatingTransportationViewModel.ShowDialog();

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
            var state = await _context.StateOrders.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Name == _stateOrder.Name && s.SoftDeleted);
            if (state != null) 
            {
                _stateOrder.StateOrderId = state.StateOrderId;
                state.SetFields(_stateOrder);    
            } 
            else await _context.AddAsync(_stateOrder);
        }

        protected override async Task updateEntity()
        {
            var state = await _context.StateOrders.FindAsync(_stateOrder.StateOrderId);
            state.SetFields(_stateOrder);
        }

        public override async Task<IEntity> GetEntity() => await _context.StateOrders.FindAsync(_stateOrder.StateOrderId);
    }
}
