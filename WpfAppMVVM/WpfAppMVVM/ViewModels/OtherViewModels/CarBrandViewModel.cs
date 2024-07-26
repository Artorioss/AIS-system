using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CarBrandViewModel : BaseViewModel
    {
        private CarBrand _brand;
        public DelegateCommand AddCarCommand { get; set; }
        public DelegateCommand AddCarByKeyboardCommand { get; set; }
        public AsyncCommand GetCarSourceAsyncCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public ObservableCollection<Car> Cars 
        {
            get => _brand.Cars;
            set 
            {
                _brand.Cars = value;
                OnPropertyChanged(nameof(Cars));
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
        public string Name 
        {
            get => _brand.Name;
            set 
            {
                _brand.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string RussianName
        {
            get => _brand.RussianName;
            set 
            {
                _brand.RussianName = value;
                OnPropertyChanged(nameof(RussianName));
            }
        }

        private Car _car;
        public Car SelectedCar 
        {
            get => _car;
            set 
            {
                _car = value;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }

        private string _windowName = "Добавление бренда автомобиля";
        public string WindowName 
        {
            get => _windowName;
            set 
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public CarBrandViewModel() 
        {
            mode = Mode.Additing;
            _brand = new CarBrand();
            Cars = new ObservableCollection<Car>();
        }

        public CarBrandViewModel(CarBrand brand) 
        {
            mode = Mode.Editing;
            _brand = brand;
            Cars = new ObservableCollection<Car>(_brand.Cars);
            WindowName = "Редактирование бренда";
        }

        protected override async Task loadReferenceData()
        {
            if (!_context.Entry(_brand).Collection(b => b.Cars).IsLoaded)
            {
                _brand.Cars.Clear();
                await loadCars(_brand);
            }
        }

        private async Task loadCars(CarBrand carBrand) 
        {
            await _context.Entry(carBrand).Collection(cb => cb.Cars).LoadAsync();
        }

        protected override void cloneEntity()
        {
            _brand = _brand.Clone() as CarBrand;
        }

        protected override void setCommands() 
        {
            AddCarCommand = new DelegateCommand((obj) => addCar());
            AddCarByKeyboardCommand = new DelegateCommand(addCarByKeyboard);
            GetCarSourceAsyncCommand = new AsyncCommand(getCarsAsync);
            DeleteCommand = new DelegateCommand(deleteCar);
        }

        private void addCar() 
        {
            if (SelectedCar != null) 
            {
                if (Cars.Contains(SelectedCar)) MessageBox.Show($"Транспорт {SelectedCar.Number} уже числится за брендом {_brand.Name}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (!string.IsNullOrEmpty(Name))
                {
                    Cars.Add(SelectedCar);
                    _brand.Cars.Add(SelectedCar);
                    if (mode == Mode.Additing) SelectedCar.Brand = new CarBrand() { Name = this.Name };
                    else if (mode == Mode.Editing) SelectedCar.BrandId = _brand.CarBrandId;
                    SelectedCar = null;
                }
                else 
                {
                    MessageBox.Show($"Неверно заполнено поле 'Наименование бренда' - {_brand.Name}. Чтобы добавить автомобиль, следует, сначала, корректно указать бренд.", "Укажите бренд", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void addCarByKeyboard(object e) 
        {
            if ((Key)e == Key.Enter) addCar();   
        }

        private void deleteCar(object obj) 
        {
            _brand.Cars.Remove(obj as Car);
            Cars.Remove(obj as Car);
        }

        private async Task getCarsAsync(object obj) 
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text)) 
            {
                CarSource = await _context.Cars
                                    .Include(c => c.Brand)
                                    .Where(c => c.Number.ToLower().Contains(text.ToLower()))
                                    .Take(5)
                                    .ToListAsync();
            }
        }

        protected override async Task<bool> dataIsCorrect() 
        {
            if (string.IsNullOrEmpty(_brand.Name)) 
            {
                MessageBox.Show($"Неверно заполнено поле 'Наименование бренда' - {_brand.Name}.","Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override async Task addEntity()
        {
            var cb = await _context.CarBrands.IgnoreQueryFilters().FirstOrDefaultAsync(cb => cb.Name == _brand.Name && cb.SoftDeleted);
            if (cb != null) 
            {
                _brand.CarBrandId = cb.CarBrandId;
                cb.SetFields(_brand);
            } 
            else await _context.AddAsync(_brand);
        }

        protected override async Task updateEntity()
        {
            var br = await _context.CarBrands.FindAsync(_brand.CarBrandId);
            br.SetFields(_brand);
        }

        public override async Task<IEntity> GetEntity()
        {
            return await _context.CarBrands.FindAsync(_brand.CarBrandId);
        }
    }
}
