using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class RoutePointViewModel: ReferenceBook
    {
        RoutePoint _routePoint;
        MonthService _monthService;
        public ObservableCollection<Route> Routes { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }

        public RoutePointViewModel()
        {
            _routePoint = new RoutePoint();
            mode = Mode.Additing;
            GroupElementsIsEnabled = false;
            settingUp();
        }

        public RoutePointViewModel(RoutePoint routePoint)
        {
            _routePoint = routePoint.Clone() as RoutePoint;
            mode = Mode.Editing;
            settingUp();
            GroupElementsIsEnabled = Routes.Count > 0;
            WindowName = "Редактирование точки маршрута";
        }

        private void settingUp()
        {
            Routes = new ObservableCollection<Route>();
            _monthService = new MonthService();
            Years = new List<int>();
            Months = new List<string>();
            setYears();
            setSelectedYear();
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
            get => Routes.Count;
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

        private bool groupElementsIsEnabled = true;
        public bool GroupElementsIsEnabled
        {
            get => groupElementsIsEnabled;
            set
            {
                groupElementsIsEnabled = value;
                OnPropertyChanged(nameof(GroupElementsIsEnabled));
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
            _months = _monthService.GetMonths(_context.Routes
                                                      .Include(r => r.RoutePoints)
                                                      .Include(r => r.Transportations)
                                                      .Where(r => r.RoutePoints.Any(rp => rp.RoutePointId == _routePoint.RoutePointId) && r.Transportations.Any(tr => tr.DateLoading.Value.Year == year))
                                                      .SelectMany(t => t.Transportations)
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

        private void setYears()
        {
            Years = _context.Routes
                            .Include(r => r.RoutePoints)
                            .Include(r => r.Transportations)
                            .Where(r => r.RoutePoints.Any(rp => rp.RoutePointId == _routePoint.RoutePointId))
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

            var list = _context.Routes
                               .Include(r => r.RoutePoints)
                               .Include(r => r.Transportations)
                               .Where(r => r.Transportations.Count > 0 && r.Transportations.Any(t => t.DateLoading.HasValue &&
                                                                          t.DateLoading.Value >= startDate &&
                                                                          t.DateLoading.Value < endDate) &&
                                           r.RoutePoints.Any(rp => rp.Name == _routePoint.Name));

            loadData(list);
            OnPropertyChanged(nameof(CountRoutes));
        }

        private void onSort()
        {
            SelectedRoute = null;
        }

        private void loadData(IQueryable<Route> list)
        {
            Routes.Clear();
            foreach (var item in list) Routes.Add(item);
        }

        protected override void addEntity()
        {
            _context.Add(_routePoint);
        }

        private void deleteEntity(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Маршурт - '{(obj as Route).RouteName}' будет удален.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) delete(obj as Route);
        }

        private void delete(Route route)
        {
            _context.Routes.Remove(_context.Routes.Single(r => r.RouteId == route.RouteId));
            _context.SaveChanges();
            Routes.Remove(route);
        }

        protected override bool dataIsCorrect()
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
            DeleteCommand = new DelegateCommand(deleteEntity);
            SortCommand = new DelegateCommand((obj) => onSort());
        }

        protected override void updateEntity()
        {
            var cm = _context.RoutePoints.Find(_routePoint.RoutePointId);
            cm.SetFields(_routePoint);
        }
    }
}
