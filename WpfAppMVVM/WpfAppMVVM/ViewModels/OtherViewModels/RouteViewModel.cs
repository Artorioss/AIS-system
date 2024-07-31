using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    public class RouteViewModel: BaseViewModel
    {
        Route _route;
        MonthService _monthService;
        public ObservableCollection<Transportation> Transportations { get; set; }
        public AsyncCommand DeleteAsyncCommand { get; set; }
        public AsyncCommand ShowWindowAsyncCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }

        public RouteViewModel(TransportationEntities Context, IDisplayRootRegistry displayRootRegistry) : base(Context, displayRootRegistry)
        {
            _route = new Route();
            mode = Mode.Additing;
            settingUp();
        }

        public RouteViewModel(Route route, TransportationEntities Context, IDisplayRootRegistry displayRootRegistry) : base(Context, displayRootRegistry)
        {
            mode = Mode.Editing;
            _route = route;
            settingUp();
            WindowName = "Редактирование маршрута";
        }

        protected override void cloneEntity()
        {
            _route = _route.Clone() as Route;
        }

        protected override async Task loadReferenceData()
        {
            if (!_context.Entry(_route).Collection(r => r.Transportations).IsLoaded)
            {
                _route.Transportations.Clear();
                await _context.Entry(_route).Collection(r => r.Transportations).LoadAsync();
            }
            
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

        private string _windowName = "Создание маршрута";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public string RouteName
        {
            get => _route.RouteName;
            set
            {
                _route.RouteName = value;
                OnPropertyChanged(nameof(RouteName));
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

        private void setMonthsByYear(int year)
        {
            _months = _monthService.GetMonths(getMonthsByYear(year));
            setSelectedMonth();
            OnPropertyChanged(nameof(Months));
        }

        private List<int> getMonthsByYear(int year) 
        {
            return _route.Transportations
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
            Years = _route.Transportations
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

            var list = _route.Transportations
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

        private async Task deleteEntity(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{(obj as Transportation).RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) await delete(obj as Transportation);
        }

        private async Task delete(Transportation dto)
        {
            _context.Transportations.Remove(_context.Transportations.Single(t => t.TransportationId == dto.TransportationId));
            await SaveChangesAsync();
            Transportations.Remove(dto);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if (string.IsNullOrEmpty(_route.RouteName))
            {
                MessageBox.Show("Неверно указан маршрут!.", "Неправильно заполнены данные.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override void setCommands()
        {
            DeleteAsyncCommand = new AsyncCommand(deleteEntity);
            ShowWindowAsyncCommand = new AsyncCommand(async (obj) => await showWindowForEdit());
            SortCommand = new DelegateCommand((obj) => onSort());
        }

        private async Task showWindowForEdit()
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
            var route = await _context.Routes.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.RouteName == _route.RouteName && r.SoftDeleted);
            if (route != null) 
            {
                _route.RouteId = route.RouteId;
                route.SetFields(_route);
            }
            else await _context.AddAsync(_route);
        }

        protected override async Task updateEntity()
        {
            var route = await _context.Routes.FindAsync(_route.RouteId);
            route.SetFields(_route);
        }

        public override async Task<IEntity> GetEntity() => await _context.Routes.FindAsync(_route.RouteId);
    }
}
