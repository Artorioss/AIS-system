using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CarViewModel : BaseViewModel
    {
        Car _car;
        public ObservableHashSet<Driver> Drivers { get; set; }
        public AsyncCommand GetDriversAsyncCommand { get; set; }
        public DelegateCommand AddDriverCommand { get; set; }
        public DelegateCommand AddDriverByKeyboardCommand { get; set; }
        public AsyncCommand GetBrandsAsyncCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public CarViewModel()
        {
            mode = Mode.Additing;
            _car = new Car();
            Drivers = new ObservableHashSet<Driver>();
        }

        public CarViewModel(Car car)
        {
            mode = Mode.Editing;
            _car = car;
            WindowName = "Редактирование автомобиля";
            BrandSource = new List<CarBrand>() { car.Brand };
            Drivers = [.. _car.Drivers];
        }

        protected override void cloneEntity()
        {
            _car = _car.Clone() as Car;
        }

        protected override async Task loadReferenceData()
        {
            if (!_context.Entry(_car).Collection(b => b.Drivers).IsLoaded)
            {
                _car.Drivers.Clear();
                await loadDrivers(_car);
            }
        }

        private async Task loadDrivers(Car car) 
        {
            await _context.Entry(car).Collection(c => c.Drivers).LoadAsync();
        }

        private string _windowName = "Создание автомобиля";
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

        public string CarNumber
        {
            get => _car.Number;
            set 
            {
                _car.Number = value;
                OnPropertyChanged(nameof(CarNumber));
            }
        }

        private List<CarBrand> _brandSource;
        public List<CarBrand> BrandSource
        {
            get => _brandSource;
            set 
            {
                _brandSource = value;
                OnPropertyChanged(nameof(BrandSource));
            }
        }

        public CarBrand SelectedCarBrand
        {
            get => _car.Brand;
            set
            {
                _car.Brand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
            }
        }

        private string _brandText;
        public string BrandText
        {
            get => _brandText;
            set 
            {
                _brandText = value;
                OnPropertyChanged(nameof(BrandText));
            }
        }

        private async Task getBrandsAsync(object obj) 
        {
            string text = obj as string;
            BrandSource = await _context.CarBrands.Where(cb => cb.Name.ToLower().Contains(text)
                                               || cb.RussianName.ToLower().Contains(text))
                                        .ToListAsync();
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

        private async Task getDriversAsync(object obj) 
        {
            string text = obj as string;
            DriverSource = await _context.Drivers
                                   .Where(d => d.Name.Contains(text.ToLower()))
                                   .Take(5)
                                   .ToListAsync();
        }

        private void addDriver()
        {
            if (string.IsNullOrEmpty(DriverName))
            {
                MessageBox.Show("Укажите инициалы водителя.", "Не указаны инициалы водителя", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedDriver is null) createDriver();
            if (Drivers.Contains(SelectedDriver)) MessageBox.Show($"Водитель {SelectedDriver.Name} уже числится за транспортом {_car.Number}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                Drivers.Add(SelectedDriver);
                _car.Drivers.Add(SelectedDriver);
            }
            SelectedDriver = null;
        }

        private void addDriverByKeyboard(object obj) 
        {
            if ((Key)obj == Key.Enter) addDriver();
        }

        private async Task createDriver()
        {
            SelectedDriver = DriverSource.SingleOrDefault(d => d.Name.ToLower() == DriverName.ToLower());
            if (SelectedDriver is null) SelectedDriver = await _context.Drivers.SingleOrDefaultAsync(d => d.Name.ToLower() == DriverName.ToLower());
            if (SelectedDriver is null) SelectedDriver = new Driver { Name = DriverName };
        }

        private async Task createBrand()
        {
            SelectedCarBrand = BrandSource.SingleOrDefault(b => b.Name.ToLower() == BrandText.ToLower() || b.RussianName.ToLower() == BrandText.ToLower());
            if (SelectedCarBrand is null) SelectedCarBrand = await _context.CarBrands.SingleOrDefaultAsync(b => b.Name.ToLower() == BrandText.ToLower() || b.RussianName.ToLower() == BrandText.ToLower());
            if (SelectedCarBrand is null) SelectedCarBrand = new CarBrand { Name = BrandText};
        }

        private void deleteEntity(object obj) 
        {
            Drivers.Remove(obj as Driver);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if (string.IsNullOrWhiteSpace(BrandText))
            {
                MessageBox.Show("Неверно указан бренд автомобиля.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var driver = Drivers.FirstOrDefault(d => d.Name == null);
            if (driver != null)
            {
                MessageBox.Show($"Неверно указаны инициалы водителя '{driver.Name}'.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (SelectedCarBrand is null) await createBrand();


            if (mode == Mode.Additing) 
            {
                if (string.IsNullOrEmpty(_car.Number) || _car.Number.Length < 8 || _car.Number.Length > 9) 
                {
                    MessageBox.Show("Неверно указан номер автомобиля. Минимальная длина: 8 символов, максимальная длина: 9 символов.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        protected override void setCommands()
        {
            GetDriversAsyncCommand = new AsyncCommand(getDriversAsync);
            AddDriverCommand = new DelegateCommand((obj) => addDriver());
            AddDriverByKeyboardCommand = new DelegateCommand(addDriverByKeyboard);
            GetBrandsAsyncCommand = new AsyncCommand(getBrandsAsync);
            DeleteCommand = new DelegateCommand(deleteEntity);
        }

        protected override async Task addEntity()
        {
            var car = await _context.Cars.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Number == _car.Number && c.SoftDeleted);
            if (car != null) car.SetFields(_car);
            else await _context.AddAsync(_car);
        }

        protected override async Task updateEntity()
        {
            var car = await _context.Cars.FindAsync(_car.Number);
            car.SetFields(_car);
        }

        public override async Task<IEntity> GetEntity() => await _context.Cars.FindAsync(_car.Number);
    }
}
