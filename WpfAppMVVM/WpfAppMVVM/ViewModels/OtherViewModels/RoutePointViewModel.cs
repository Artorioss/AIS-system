using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class RoutePointViewModel: ReferenceBook
    {
        RoutePoint _routePoint;
        MonthService _monthService;
        public AsyncCommand DeleteAsyncCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }
        public ObservableCollection<Route> FilteredRoutes { get; private set; }

        public RoutePointViewModel()
        {
            _routePoint = new RoutePoint();
            mode = Mode.Additing;
            settingUp();
        }

        public RoutePointViewModel(RoutePoint routePoint)
        {
            mode = Mode.Editing;
            _routePoint = routePoint;
            settingUp();
            WindowName = "Редактирование точки маршрута";
        }

        protected override void cloneEntity()
        {
            _routePoint = _routePoint.Clone() as RoutePoint;
        }

        protected override async Task loadReferenceData()
        {
            if (!_context.Entry(_routePoint).Collection(rp => rp.Routes).IsLoaded)
            {
                _routePoint.Routes.Clear();
                await _context.Entry(_routePoint).Collection(rp => rp.Routes).LoadAsync();
            }
            foreach (var route in _routePoint.Routes) 
            {
                if (!_context.Entry(route).Collection(r => r.Transportations).IsLoaded) 
                {
                    await _context.Entry(route).Collection(r => r.Transportations).LoadAsync();
                }
            }
            setYears();
            setSelectedYear();
            OnPropertyChanged(nameof(GroupElementsIsEnabled));
        }

        private void settingUp()
        {
            FilteredRoutes = new ObservableCollection<Route>();
            _monthService = new MonthService();
            Years = new List<int>();
            Months = new List<string>();
        }

        private string _windowName = "Создание точки маршрута";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public string RoutePointName
        {
            get => _routePoint.Name;
            set
            {
                _routePoint.Name = value;
                OnPropertyChanged(nameof(RoutePointName));
            }
        }

        public int CountRoutes
        {
            get => FilteredRoutes.Count;
        }

        private Route _route;
        public Route SelectedRoute
        {
            get => _route;
            set
            {
                _route = value;
                OnPropertyChanged(nameof(SelectedRoute));
            }
        }

        public bool GroupElementsIsEnabled
        {
            get => CountRoutes > 0; 
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
            _months = _monthService.GetMonths(getMonthsbyYear(year));
            setSelectedMonth();
            OnPropertyChanged(nameof(Months));
        }

        private List<int> getMonthsbyYear(int year) 
        {
            return  _routePoint.Routes
                    .Where(r => r.Transportations.Any(tr => tr.DateLoading.Value.Year == year))
                    .SelectMany(t => t.Transportations)
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
            Years = _routePoint.Routes
                           .SelectMany(r => r.Transportations)
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

            var list = _routePoint.Routes.Where(r => r.Transportations.Count > 0 && r.Transportations.Any(t => t.DateLoading.HasValue &&
                                                                          t.DateLoading.Value >= startDate &&
                                                                          t.DateLoading.Value < endDate));
            loadData(list);
            OnPropertyChanged(nameof(CountRoutes));
        }

        private void onSort()
        {
            SelectedRoute = null;
        }

        private void loadData(IEnumerable<Route> list)
        {
            FilteredRoutes.Clear();
            foreach (var item in list) FilteredRoutes.Add(item);
        }

        private async Task deleteEntity(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Маршурт - '{(obj as Route).RouteName}' будет удален.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) await delete(obj as Route);
        }

        private async Task delete(Route route)
        {
            _context.Routes.Remove(_context.Routes.Single(r => r.RouteId == route.RouteId));
            await SaveChangesAsync();
            FilteredRoutes.Remove(route);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if (string.IsNullOrEmpty(_routePoint.Name))
            {
                MessageBox.Show("Неверно указано название пункта маршрута.", "Неправильно заполнены данные.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override void setCommands()
        {
            DeleteAsyncCommand = new AsyncCommand(deleteEntity);
            SortCommand = new DelegateCommand((obj) => onSort());
        }

        protected override async Task addEntity()
        {
            var routePoint = await _context.RoutePoints.SingleOrDefaultAsync(rp => rp.Name == _routePoint.Name && rp.SoftDeleted);
            if (routePoint != null) routePoint.SetFields(_routePoint);
            else await _context.AddAsync(_routePoint);
        }

        protected override async Task updateEntity()
        {
            var cm = await _context.RoutePoints.FindAsync(_routePoint.RoutePointId);
            cm.SetFields(_routePoint);
        }

        public override async Task<IEntity> GetEntity() => await _context.RoutePoints.FindAsync(_routePoint.RoutePointId);
    }
}
