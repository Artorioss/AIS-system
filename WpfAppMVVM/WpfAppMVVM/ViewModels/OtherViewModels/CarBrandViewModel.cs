using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CarBrandViewModel : ReferenceBook
    {
        private CarBrand _brand;
        public ObservableCollection<Car> Cars { get; set; }
        public DelegateCommand AddCarCommand { get; set; }
        public DelegateCommand AddCarByKeyboardCommand { get; set; }
        public DelegateCommand GetCarSourceCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

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

            _context.Entry(brand).Collection(b => b.Cars).Load();

            _brand = brand.Clone() as CarBrand;
            Cars = new ObservableCollection<Car>(_brand.Cars);
            WindowName = "Редактирование бренда";
        }

        protected override void setCommands() 
        {
            AddCarCommand = new DelegateCommand((obj) => addCar());
            AddCarByKeyboardCommand = new DelegateCommand(addCarByKeyboard);
            GetCarSourceCommand = new DelegateCommand(getCars);
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

        private void getCars(object obj) 
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text)) 
            {
                CarSource = _context.Cars
                                    .Include(c => c.Brand)
                                    .Where(c => c.Number.ToLower().Contains(text.ToLower()))
                                    .Take(5)
                                    .ToList();
            }
        }

        protected override bool dataIsCorrect() 
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
            var cb = await _context.CarBrands.FirstOrDefaultAsync(cb => cb.Name == _brand.Name && cb.SoftDeleted);
            if (cb != null) cb.SetFields(_brand);
            else await _context.AddAsync(_brand);
        }

        protected override async Task updateEntity()
        {
            var br = await _context.CarBrands.FindAsync(_brand.CarBrandId);
            br.SetFields(_brand);
        }

        public override IEntity GetEntity()
        {
            return _brand;
        }
    }
}
