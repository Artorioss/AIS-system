using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class DriverViewModel : ReferenceBook
    {
        public DelegateCommand GetCustomers { get; private set; }
        public DelegateCommand GetCars { get; private set; }
        public DelegateCommand GetTraillers { get; private set; }
        public DelegateCommand AddCar { get; private set; }
        public DelegateCommand AddCarByKeyboard { get; private set; }
        public DelegateCommand DeleteCar { get; private set; }
        public DelegateCommand AddTrailler { get; private set; }
        public DelegateCommand AddTraillerByKeyboard { get; private set; }
        public DelegateCommand GetCarBrands { get; private set; }
        public DelegateCommand GetTraillerBrands { get; private set; }
        public DelegateCommand DeleteTrailler { get; private set; }

        private Driver _driver;

        public ObservableCollection<Car> Cars { get; private set; }  //Коллекции для DataGridView
        public ObservableCollection<Trailler> Traillers { get; private set; }

        private List<Car> _carSource;
        public List<Car> CarSource //Коллекция для comboBoxCarNumbers
        {
            get => _carSource;
            set 
            {
                _carSource = value;
                OnPropertyChanged(nameof(CarSource));
            }
        }

        private Car _car;
        public Car Car //Выбранный элемент в dataGridViewCars
        {
            get => _car;
            set 
            {
                _car = value;
                OnPropertyChanged(nameof(Car));
            }
        }

        private string _carText;
        public string CarText  //Текст в comboBoxCarNumbers
        {
            get => _carText;
            set 
            {
                _carText = value;
                OnPropertyChanged(nameof(CarText));
            }
        }

        private List<CarBrand> _carBrandSource;
        public List<CarBrand> CarBrandSource //Коллекция для CustomComboBox вcтроенной в DataGridViewCars
        {
            get => _carBrandSource;
            set 
            {
                _carBrandSource = value;
                OnPropertyChanged(nameof(CarBrandSource));
            }
        }

        public CarBrand SelectedCarBrand 
        {
            get => Car.Brand;
            set 
            {
                if (Car != null) 
                {
                    Car.Brand = value;
                    OnPropertyChanged(nameof(SelectedCarBrand));
                }
                
            }
        }

        private List<TraillerBrand> _traillerBrandSource;
        public List<TraillerBrand> TraillerBrandSource 
        {
            get => _traillerBrandSource;
            set 
            {
                _traillerBrandSource = value;
                OnPropertyChanged(nameof(TraillerBrandSource));
            }
        }

        public TraillerBrand SelectedTraillerBrand
        {
            get => Trailler.Brand;
            set
            {
                if (Trailler != null) 
                {
                    Trailler.Brand = value;
                    OnPropertyChanged(nameof(SelectedTraillerBrand));
                }   
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
            Cars = new ObservableCollection<Car>();
            Traillers = new ObservableCollection<Trailler>();
            _context.Add(_driver);
            mode = Mode.Additing;
        }

        public DriverViewModel(Driver driver)
        {
            _context.Entry(driver).Collection(d => d.Cars).Load();
            foreach (Car car in driver.Cars) if (car.Brand is null) _context.Entry(car).Reference(c => c.Brand).Load();

            _context.Entry(driver).Collection(d => d.Traillers).Load();
            foreach (Trailler trailler in driver.Traillers) _context.Entry(trailler).Reference(t => t.Brand).Load();

            _driver = driver.Clone() as Driver;

            Cars = new ObservableCollection<Car>(_driver.Cars);
            Traillers = new ObservableCollection<Trailler>(_driver.Traillers);

            TransportCompanySource = new List<TransportCompany>() { _driver.TransportCompany };
            WindowName = "Редактирование записи";
            mode = Mode.Editing;
        }

        protected override void setCommands() 
        {
            GetCustomers = new DelegateCommand(getCustomers);
            GetCars = new DelegateCommand(getCars);
            GetTraillers = new DelegateCommand(getTraillers);
            AddCar = new DelegateCommand((obj) => addCar());
            AddCarByKeyboard = new DelegateCommand(addCarByKeyboard);
            DeleteCar = new DelegateCommand(deleteCar);
            AddTrailler = new DelegateCommand((obj) => addTrailler());
            AddTraillerByKeyboard = new DelegateCommand(addTraillerByKeyboard);
            DeleteTrailler = new DelegateCommand(deleteTrailler);
            GetCarBrands = new DelegateCommand(getCarBrands);
            GetTraillerBrands = new DelegateCommand(getTraillerBrands);
        }

        private void getCarBrands(object obj) 
        {
            string text = obj as string;
            CarBrandSource = getCarBrands(text);
        }

        private void getTraillerBrands(object obj)
        {
            string text = obj as string;
            TraillerBrandSource = getTraillerBrands(text);
        }

        private List<CarBrand> getCarBrands(string text) 
        {
            return _context.CarBrands
                           .Where(b => b.Name.ToLower().Contains(text.ToLower()) || b.RussianName.ToLower().Contains(text.ToLower()))
                           .Take(5)
                           .ToList();
        }


        private List<TraillerBrand> getTraillerBrands(string text) 
        {
            return _context.TraillerBrands
                           .Where(b => b.Name.ToLower().Contains(text.ToLower()) || b.RussianName.ToLower().Contains(text.ToLower()))
                           .Take(5)
                           .ToList();
        }

        private void getCustomers(object obj)
        {
            string text = obj as string;
            TransportCompanySource = _context.TransportCompanies
                                             .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                                             .Take(5)
                                             .ToList();
        }

        private void getCars(object obj) 
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text)) 
            {
                CarSource = _context.Cars
                                    .Include(car => car.Brand)
                                    .Where(c => c.Number.ToLower().Contains(text.ToLower()))
                                    .Take(5)
                                    .ToList();
            }
        }

        private void getTraillers(object obj) 
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text)) 
            {
                TraillerSource = _context.Traillers
                                         .Include(trailler => trailler.Brand)
                                         .Where(t => t.Number.ToLower().Contains(text.ToLower()))
                                         .Take(5)
                                         .ToList();
            }
        }

        private void addCar() 
        {
            if (string.IsNullOrEmpty(CarText)) 
            {
                MessageBox.Show("Укажите номер автомобиля", "Не указан номер автомобиля", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 
            if (Car is null) createCar();
            if (Cars.Contains(Car)) MessageBox.Show("Указанное транспортное средство уже числится за этим водителем", "Невозможно выполнить действие", MessageBoxButton.OK, MessageBoxImage.Warning);
            else 
            {
                Cars.Add(Car);
                _driver.Cars.Add(Car);
            } 
            Car = null;
        }

        private void addTrailler() 
        {
            if (string.IsNullOrEmpty(TraillerText))
            {
                MessageBox.Show("Укажите номер прицепа", "Не указан номер прицепа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Trailler is null) createTrailler();
            if (Traillers.Contains(Trailler)) MessageBox.Show("Указанный прицеп уже числится за этим водителем", "Невозможно выполнить действие", MessageBoxButton.OK, MessageBoxImage.Warning);
            else 
            {
                Traillers.Add(Trailler);
                _driver.Traillers.Add(Trailler);
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
        private void createCar()
        {
            Car = CarSource.SingleOrDefault(car => car.Number.ToLower() == CarText.ToLower());
            if (Car is null) Car = _context.Cars.SingleOrDefault(car => car.Number.ToLower() == CarText.ToLower());
            if (Car is null) Car = new Car { Number = CarText };
        }

        //Поиск прицепа по номеру
        private void createTrailler() 
        {
            Trailler = TraillerSource.SingleOrDefault(trailler => trailler.Number.ToLower() == TraillerText.ToLower());
            if (Trailler is null) Trailler = _context.Traillers.SingleOrDefault(trailler => trailler.Number.ToLower() == TraillerText.ToLower());
            if (Trailler is null) Trailler = new Trailler { Number = TraillerText };
        }

        private void deleteCar(object obj) 
        {
            _driver.Cars.Remove(obj as Car);
            Cars.Remove(obj as Car);
        }

        private void deleteTrailler(object obj)
        {
            _driver.Traillers.Remove(obj as Trailler);
            Traillers.Remove(obj as Trailler);
        }

        protected override bool dataIsCorrect()
        {
            if(string.IsNullOrWhiteSpace(Name)) 
            {
                MessageBox.Show("Неверно указаны инициалы водителя.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (TransportCompany is null) 
            {
                MessageBox.Show("Неверно указана компания водителя.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var car = Cars.FirstOrDefault(c => c.Brand == null); 
            if(car != null) 
            {
                MessageBox.Show($"Неверно указан бренд для автомобиля '{car.Number}'.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override void updateEntity()
        {
            var dr = _context.Drivers.Find(_driver.DriverId);
            dr.SetFields(_driver);
        }

        protected override void addEntity()
        {
            _context.Add(_driver);
        }
    }
}
