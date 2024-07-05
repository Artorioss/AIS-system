using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class TransportCompanyViewModel: ReferenceBook
    {
        TransportCompany _transportCompany;
        public ObservableHashSet<Driver> Drivers { get; set; }
        public DelegateCommand GetDriversCommand { get; set; }
        public DelegateCommand AddDriverCommand { get; set; }
        public DelegateCommand AddDriverByKeyboardCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public TransportCompanyViewModel()
        {
            mode = Mode.Additing;
            _transportCompany = new TransportCompany();
            Drivers = new ObservableHashSet<Driver>();
        }

        public TransportCompanyViewModel(TransportCompany transportCompany)
        {
            mode = Mode.Editing;
            _context.Entry(transportCompany).Collection(c => c.Drivers).Load();
            _transportCompany = transportCompany.Clone() as TransportCompany;
            WindowName = "Редактирование транспортной компании";
            Drivers = new ObservableHashSet<Driver>(_transportCompany.Drivers);
        }

        private string _windowName = "Создание транспортной компании";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public bool NumberIsEnabled
        {
            get => mode == Mode.Additing;
        }

        public string TransportCompanyName
        {
            get => _transportCompany.Name;
            set
            {
                _transportCompany.Name = value;
                OnPropertyChanged(nameof(TransportCompanyName));
            }
        }

        private List<Driver> _drivers;
        public List<Driver> DriverSource
        {
            get => _drivers;
            set
            {
                _drivers = value;
                OnPropertyChanged(nameof(DriverSource));
            }
        }

        private string _driverName;
        public string DriverName
        {
            get => _driverName;
            set
            {
                _driverName = value;
                OnPropertyChanged(nameof(DriverName));
            }
        }

        private Driver _selectedDriver;
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }

        private void getDrivers(object obj)
        {
            string text = obj as string;
            DriverSource = _context.Drivers
                                   .Where(d => d.Name.Contains(text))
                                   .Take(5)
                                   .ToList();
        }

        private void addDriver()
        {
            if (string.IsNullOrEmpty(DriverName))
            {
                MessageBox.Show("Укажите инициалы водителя.", "Не указаны инициалы водителя", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedDriver is null) createDriver();
            if (Drivers.Contains(SelectedDriver)) MessageBox.Show($"Водитель {SelectedDriver.Name} уже числится за компанией {_transportCompany.Name}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                Drivers.Add(SelectedDriver);
                _transportCompany.Drivers.Add(SelectedDriver);
            }
            SelectedDriver = null;
        }

        private void addDriverByKeyboard(object obj)
        {
            if ((Key)obj == Key.Enter) addDriver();
        }

        private void createDriver()
        {
            SelectedDriver = DriverSource.SingleOrDefault(d => d.Name.ToLower() == DriverName.ToLower());
            if (SelectedDriver is null) SelectedDriver = _context.Drivers.SingleOrDefault(d => d.Name.ToLower() == DriverName.ToLower());
            if (SelectedDriver is null) SelectedDriver = new Driver { Name = DriverName };
        }

        protected override void addEntity()
        {
            _context.Add(_transportCompany);
        }

        private void deleteEntity(object obj)
        {
            _context.Drivers.Remove(obj as Driver);
        }

        protected override bool dataIsCorrect()
        {
            if (string.IsNullOrEmpty(TransportCompanyName)) 
            {
                MessageBox.Show($"Неверно указано наименование транспортной компании.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var driver = Drivers.FirstOrDefault(d => d.Name == null);
            if (driver != null)
            {
                MessageBox.Show($"Неверно указаны инициалы водителя '{driver.Name}'.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        protected override void setCommands()
        {
            GetDriversCommand = new DelegateCommand(getDrivers);
            AddDriverCommand = new DelegateCommand((obj) => addDriver());
            AddDriverByKeyboardCommand = new DelegateCommand(addDriverByKeyboard);
            DeleteCommand = new DelegateCommand(deleteEntity);
        }

        protected override void updateEntity()
        {
            var tc = _context.TransportCompanies.Find(_transportCompany.TransportCompanyId);
            tc.SetFields(_transportCompany);
        }
    }
}
