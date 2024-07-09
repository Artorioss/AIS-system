using System.Collections.ObjectModel;
using System.Windows;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class RouteViewModel: ReferenceBook
    {
        Route _route;
        MonthService _monthService;
        public ObservableCollection<Transportation> Transportations { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand ShowWindowCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }

        public RouteViewModel()
        {
            _route = new Route();
            mode = Mode.Additing;
            settingUp();
        }

        public RouteViewModel(Route route)
        {
            _route = route.Clone() as Route;
            mode = Mode.Editing;;
            settingUp();
            WindowName = "Редактирование маршрута";
        }

        private void settingUp()
        {
            _monthService = new MonthService();
            Years = new List<int>();
            Months = new List<string>();
            Transportations = new ObservableCollection<Transportation>();
            setYears();
            setSelectedYear();
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
            _months = _monthService.GetMonths(_context.Transportations
                                                  .Where(t => t.DateLoading.Value.Year == year
                                                         && t.RouteId == _route.RouteId)
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
            Years = _context.Transportations
                            .Where(t => t.RouteId == _route.RouteId)
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

            var list = _context.Transportations
                .Where(t => t.DateLoading.HasValue &&
                            t.DateLoading.Value >= startDate &&
                            t.DateLoading.Value < endDate
                            && t.RouteId == _route.RouteId);

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

        protected override void addEntity()
        {
            _context.Add(_route);
        }

        private void deleteEntity(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{(obj as Transportation).RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) delete(obj as Transportation);
        }

        private void delete(Transportation dto)
        {
            _context.Transportations.Remove(_context.Transportations.Single(t => t.TransportationId == dto.TransportationId));
            _context.SaveChanges();
            Transportations.Remove(dto);
        }

        protected override bool dataIsCorrect()
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
            DeleteCommand = new DelegateCommand(deleteEntity);
            ShowWindowCommand = new DelegateCommand((obj) => showWindowForEdit());
            SortCommand = new DelegateCommand((obj) => onSort());
        }

        protected override void updateEntity()
        {
            var route = _context.Routes.Find(_route.RouteId);
            route.SetFields(_route);
        }

        private void showWindowForEdit()
        {
            if (SelectedTransportation != null)
            {
                CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
                CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel((SelectedTransportation as Transportation).TransportationId);
                creatingTransportationWindow.DataContext = creatingTransportationViewModel;
                creatingTransportationWindow.ShowDialog();

                if (creatingTransportationViewModel.IsContextChanged)
                {
                    var entity = _context.Transportations.Single(tr => tr.TransportationId == creatingTransportationViewModel.Transportation.TransportationId);
                    if (creatingTransportationViewModel.Transportation.DateLoading.Value.Month != _selectedMonth || creatingTransportationViewModel.Transportation.DateLoading.Value.Year != SelectedYear)
                    {
                        DateTime date = creatingTransportationViewModel.Transportation.DateLoading.Value;
                        setDate(date);
                        getItems();
                    }
                    else
                    {
                        int id = Transportations.IndexOf(SelectedTransportation);
                        Transportations.Remove(SelectedTransportation);
                        Transportations.Insert(id, entity);
                        SelectedTransportation = entity;
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

        public override ICloneable GetEntity() => _route;
    }
}
