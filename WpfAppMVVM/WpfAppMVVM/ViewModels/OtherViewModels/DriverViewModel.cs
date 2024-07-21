using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class DriverViewModel : ReferenceBook
    {
        public AsyncCommand GetCustomers { get; private set; }
        public AsyncCommand GetCars { get; private set; }
        public AsyncCommand GetTraillers { get; private set; }
        public AsyncCommand AddCar { get; private set; }
        public DelegateCommand AddCarByKeyboard { get; private set; }
        public DelegateCommand DeleteCar { get; private set; }
        public AsyncCommand AddTrailler { get; private set; }
        public DelegateCommand AddTraillerByKeyboard { get; private set; }
        public DelegateCommand DeleteTrailler { get; private set; }

        private Driver _driver;
        public ObservableCollection<Car> Cars 
        {
            get => _driver.Cars;
            set 
            {
                _driver.Cars = value;
                OnPropertyChanged(nameof(Cars));
            }
        }
        public ObservableCollection<Trailler> Traillers 
        {
            get => _driver.Traillers;
            set 
            {
                _driver.Traillers = value;
                OnPropertyChanged(nameof(Traillers));
            }
        }

        private List<Car> _carSource;
        public List<Car> CarSource
        {
            get => _carSource;
            set 
            {
                _carSource = value;
                OnPropertyChanged(nameof(CarSource));
            }
        }

        private Car _car;
        public Car Car
        {
            get => _car;
            set 
            {
                _car = value;
                OnPropertyChanged(nameof(Car));
            }
        }

        private string _carText;
        public string CarText
        {
            get => _carText;
            set 
            {
                _carText = value;
                OnPropertyChanged(nameof(CarText));
            }
        }


        private List<Trailler> _traillerSource;
        public List<Trailler> TraillerSource
        {
            get => _traillerSource;
            set
            {
                _traillerSource = value;
                OnPropertyChanged(nameof(TraillerSource));
            }
        }

        private Trailler _trailler;
        public Trailler Trailler
        {
            get => _trailler;
            set
            {
                _trailler = value;
                OnPropertyChanged(nameof(Trailler));
            }
        }

        private string _traillerText;
        public string TraillerText
        {
            get => _traillerText;
            set
            {
                _traillerText = value;
                OnPropertyChanged(nameof(TraillerText));
            }
        }


        private List<TransportCompany> _transportCompanies;
        public List<TransportCompany> TransportCompanySource
        {
            get => _transportCompanies;
            set 
            {
                _transportCompanies = value;
                OnPropertyChanged(nameof(TransportCompanySource));
            }
        }
        public TransportCompany TransportCompany 
        {
            get => _driver.TransportCompany;
            set
            {
                _driver.TransportCompany = value;
                OnPropertyChanged(nameof(TransportCompany));
            }
        }

        private string _companyText;
        public string CompanyText 
        {
            get => _companyText;
            set 
            {
                _companyText = value;
                OnPropertyChanged(nameof(CompanyText));
            }
        }

        public string Name 
        {
            get => _driver.Name;
            set 
            {
                _driver.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _windowName = "Добавление автомобиля";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public DriverViewModel() 
        {
            _driver = new Driver();
            _context.Add(_driver);
            mode = Mode.Additing;
        }

        public DriverViewModel(Driver driver)
        {
            _driver = driver;
            TransportCompanySource = new List<TransportCompany>() { _driver.TransportCompany };
            WindowName = "Редактирование записи";
            mode = Mode.Editing;
        }
        
        protected override async Task loadReferenceData() 
        {
            if (!_context.Entry(_driver).Collection(d => d.Cars).IsLoaded) 
            {
                _driver.Cars.Clear();
                await loadCars(_driver);
            }
            if (!_context.Entry(_driver).Collection(d => d.Traillers).IsLoaded) 
            {
                _driver.Traillers.Clear();
                await loadTraillers(_driver);
            }
        }

        protected override void cloneEntity()
        {
            _driver = _driver.Clone() as Driver;
        }

        private async Task loadCars(Driver driver) 
        {
            await Application.Current.Dispatcher.InvokeAsync(_context.Entry(driver).Collection(d => d.Cars).Load);
            foreach (Car car in driver.Cars) 
            {
                await _context.Entry(car).Reference(c => c.Brand).LoadAsync();
            }
        }

        private async Task loadTraillers(Driver driver) 
        {
            await _context.Entry(driver).Collection(d => d.Traillers).LoadAsync();
            foreach (Trailler trailler in driver.Traillers) 
            {
                await _context.Entry(trailler).Reference(t => t.Brand).LoadAsync();
            }
        }

        protected override void setCommands() 
        {
            GetCustomers = new AsyncCommand(getCustomers);
            GetCars = new AsyncCommand(getCars);
            GetTraillers = new AsyncCommand(getTraillers);
            AddCar = new AsyncCommand(async (obj) => await addCar());
            AddCarByKeyboard = new DelegateCommand(addCarByKeyboard);
            DeleteCar = new DelegateCommand(deleteCar);
            AddTrailler = new AsyncCommand(async (obj) => await addTrailler());
            AddTraillerByKeyboard = new DelegateCommand(addTraillerByKeyboard);
            DeleteTrailler = new DelegateCommand(deleteTrailler);
        }

        private async Task getCustomers(object obj)
        {
            string text = obj as string;
            TransportCompanySource = await _context.TransportCompanies
                                             .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                             .Take(5)
                                             .ToListAsync();
        }

        private async Task getCars(object obj) 
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text)) 
            {
                CarSource = await _context.Cars
                                    .Include(car => car.Brand)
                                    .Where(c => c.Number.ToLower().Contains(text.ToLower()))
                                    .Take(5)
                                    .ToListAsync();
            }
        }

        private async Task getTraillers(object obj) 
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text)) 
            {
                TraillerSource = await _context.Traillers
                                         .Include(trailler => trailler.Brand)
                                         .Where(t => t.Number.ToLower().Contains(text.ToLower()))
                                         .Take(5)
                                         .ToListAsync();
            }
        }

        private async Task addCar() 
        {
            if (string.IsNullOrEmpty(CarText)) 
            {
                MessageBox.Show("Укажите номер автомобиля", "Не указан номер автомобиля", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 
            if (Car is null) await createCar();
            if (Cars.Contains(Car)) MessageBox.Show("Указанное транспортное средство уже числится за этим водителем", "Невозможно выполнить действие", MessageBoxButton.OK, MessageBoxImage.Warning);
            else 
            {
                Cars.Add(Car);
            } 
            Car = null;
        }

        private async Task addTrailler() 
        {
            if (string.IsNullOrEmpty(TraillerText))
            {
                MessageBox.Show("Укажите номер прицепа", "Не указан номер прицепа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Trailler is null) await createTrailler();
            if (Traillers.Contains(Trailler)) MessageBox.Show("Указанный прицеп уже числится за этим водителем", "Невозможно выполнить действие", MessageBoxButton.OK, MessageBoxImage.Warning);
            else 
            {
                Traillers.Add(Trailler);
            }
            Trailler = null;
        }

        private void addCarByKeyboard(object e) 
        {
            if ((Key)e == Key.Enter && CarText != null) addCar();
        }

        private void addTraillerByKeyboard(object e)
        {
            if ((Key)e == Key.Enter && TraillerText != null) addTrailler();
        }

        //Поиск машины по номеру
        private async Task createCar()
        {
            Car = CarSource.SingleOrDefault(car => car.Number.ToLower() == CarText.ToLower());
            if (Car is null) Car = await _context.Cars.SingleOrDefaultAsync(car => car.Number.ToLower() == CarText.ToLower());
            if (Car is null) Car = new Car { Number = CarText };
        }

        //Поиск прицепа по номеру
        private async Task createTrailler() 
        {
            Trailler = TraillerSource.SingleOrDefault(trailler => trailler.Number.ToLower() == TraillerText.ToLower());
            if (Trailler is null) Trailler = await _context.Traillers.SingleOrDefaultAsync(trailler => trailler.Number.ToLower() == TraillerText.ToLower());
            if (Trailler is null) Trailler = new Trailler { Number = TraillerText };
        }

        private async Task createCompany() 
        {
            TransportCompany = TransportCompanySource.SingleOrDefault(company => company.Name.ToLower() == CompanyText);
            if(TransportCompany is null) TransportCompany = await _context.TransportCompanies.SingleOrDefaultAsync(company => company.Name.ToLower() == CompanyText.ToLower());
            if (TransportCompany is null) TransportCompany = new TransportCompany { Name = CompanyText };
        }

        private void deleteCar(object obj) 
        {
            Cars.Remove(obj as Car);
        }

        private void deleteTrailler(object obj)
        {
            Traillers.Remove(obj as Trailler);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if(string.IsNullOrWhiteSpace(Name)) 
            {
                MessageBox.Show("Неверно указаны инициалы водителя.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(CompanyText)) 
            {
                MessageBox.Show("Неверно указана компания водителя.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (TransportCompany is null) await createCompany();

            var car = Cars.FirstOrDefault(c => c.Brand == null); 
            if(car != null) 
            {
                MessageBox.Show($"Неверно указан бренд для автомобиля '{car.Number}'.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override async Task addEntity()
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Name == _driver.Name && d.SoftDeleted);
            if (driver != null) driver.SetFields(_driver);
            else await _context.AddAsync(_driver);
        }

        protected override async Task updateEntity()
        {
            var dr = await _context.Drivers.FindAsync(_driver.DriverId);
            dr.SetFields(_driver);
        }

        public override async Task<IEntity> GetEntity() => await _context.Drivers.FindAsync(_driver.DriverId);
    }
}
