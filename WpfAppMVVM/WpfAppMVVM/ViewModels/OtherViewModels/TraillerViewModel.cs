using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    class TraillerViewModel: BaseViewModel
    {
        Trailler _trailler;
        public ObservableHashSet<Driver> Drivers { get; set; }
        public DelegateCommand GetDriversCommand { get; set; }
        public AsyncCommand AddDriverAsyncCommand { get; set; }
        public DelegateCommand AddDriverByKeyboardCommand { get; set; }
        public DelegateCommand GetBrandsCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public TraillerViewModel()
        {
            mode = Mode.Additing;
            _trailler = new Trailler();
            Drivers = new ObservableHashSet<Driver>();
        }

        public TraillerViewModel(Trailler trailler)
        {
            mode = Mode.Editing;
            _trailler = trailler;   
            WindowName = "Редактирование прицепа";
            BrandSource = new List<TraillerBrand>() { trailler.Brand };
            Drivers = new ObservableHashSet<Driver>(trailler.Drivers);
        }

        protected override void cloneEntity()
        {
            _trailler = _trailler.Clone() as Trailler;
        }

        protected override async Task loadReferenceData()
        {
            if (!_context.Entry(_trailler).Collection(c => c.Drivers).IsLoaded)
            {
                _trailler.Drivers.Clear();
                await _context.Entry(_trailler).Collection(c => c.Drivers).LoadAsync();
            }
        }

        private string _windowName = "Создание прицепа";
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

        public string TraillerNumber
        {
            get => _trailler.Number;
            set
            {
                _trailler.Number = value;
                OnPropertyChanged(nameof(TraillerNumber));
            }
        }

        private List<TraillerBrand> _brandSource;
        public List<TraillerBrand> BrandSource
        {
            get => _brandSource;
            set
            {
                _brandSource = value;
                OnPropertyChanged(nameof(BrandSource));
            }
        }

        public TraillerBrand SelectedBrand
        {
            get => _trailler.Brand;
            set
            {
                _trailler.Brand = value;
                OnPropertyChanged(nameof(SelectedBrand));
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

        private void getBrands(object obj)
        {
            string text = obj as string;
            BrandSource = _context.TraillerBrands.Where(cb => cb.Name.ToLower().Contains(text)
                                                    || cb.RussianName.ToLower().Contains(text))
                                                 .ToList();

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

        private async Task addDriver()
        {
            if (string.IsNullOrEmpty(DriverName))
            {
                MessageBox.Show("Укажите инициалы водителя.", "Не указаны инициалы водителя", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedDriver is null) await createDriver();
            if (Drivers.Contains(SelectedDriver)) MessageBox.Show($"Водитель {SelectedDriver.Name} уже числится за прицепом {_trailler.Number}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                Drivers.Add(SelectedDriver);
                _trailler.Drivers.Add(SelectedDriver);
            }
            SelectedDriver = null;
        }

        private async void addDriverByKeyboard(object obj)
        {
            if ((Key)obj == Key.Enter) await addDriver();
        }

        private async Task createDriver()
        {
            SelectedDriver = DriverSource.SingleOrDefault(d => d.Name.ToLower() == DriverName.ToLower());
            if (SelectedDriver is null) SelectedDriver = await _context.Drivers.SingleOrDefaultAsync(d => d.Name.ToLower() == DriverName.ToLower());
            if (SelectedDriver is null) SelectedDriver = new Driver { Name = DriverName };
        }

        private async Task createBrand()
        {
            SelectedBrand = BrandSource.SingleOrDefault(b => b.Name.ToLower() == BrandText.ToLower() || b.RussianName.ToLower() == BrandText.ToLower());
            if (SelectedBrand is null) SelectedBrand = await _context.TraillerBrands.SingleOrDefaultAsync(b => b.Name.ToLower() == BrandText.ToLower() || b.RussianName.ToLower() == BrandText.ToLower());
            if (SelectedBrand is null) SelectedBrand = new TraillerBrand { Name = BrandText };
        }

        private void deleteEntity(object obj)
        {
            Drivers.Remove(obj as Driver);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if (string.IsNullOrWhiteSpace(BrandText))
            {
                MessageBox.Show("Неверно указан бренд прицепа.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var driver = Drivers.FirstOrDefault(d => d.Name == null);
            if (driver != null)
            {
                MessageBox.Show($"Неверно указаны инициалы водителя '{driver.Name}'.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (SelectedBrand is null) await createBrand();


            if (mode == Mode.Additing)
            {
                if (string.IsNullOrEmpty(_trailler.Number) || _trailler.Number.Length < 8 || _trailler.Number.Length > 9)
                {
                    MessageBox.Show("Неверно указан номер прицепа. Минимальная длина: 8 символов, максимальная длина: 9 символов.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        protected override void setCommands()
        {
            GetDriversCommand = new DelegateCommand(getDrivers);
            AddDriverAsyncCommand = new AsyncCommand(async (obj) => await addDriver());
            AddDriverByKeyboardCommand = new DelegateCommand(addDriverByKeyboard);
            GetBrandsCommand = new DelegateCommand(getBrands);
            DeleteCommand = new DelegateCommand(deleteEntity);
        }

        protected override async Task addEntity()
        {
            var trailler = await _context.Traillers.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Number == _trailler.Number && t.SoftDeleted);
            if (trailler != null) 
            {
                _trailler.Number = trailler.Number;
                trailler.SetFields(_trailler);
            }
            else await _context.AddAsync(_trailler);
        }

        protected override async Task updateEntity()
        {
            var trailler = await _context.Traillers.FindAsync(_trailler.Number);
            trailler.SetFields(_trailler);
        }

        public override async Task<IEntity> GetEntity() => await _context.Traillers.FindAsync(_trailler.Number);
    }
}
