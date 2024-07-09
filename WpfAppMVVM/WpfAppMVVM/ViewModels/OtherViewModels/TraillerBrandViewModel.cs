using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class TraillerBrandViewModel: ReferenceBook
    {
        private TraillerBrand _brand;
        public ObservableCollection<Trailler> Traillers { get; set; }
        public DelegateCommand AddTraillerCommand { get; set; }
        public DelegateCommand AddTraillerByKeyboardCommand { get; set; }
        public DelegateCommand GetTraillerSourceCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

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

        private Trailler _trailler;
        public Trailler SelectedTrailler
        {
            get => _trailler;
            set
            {
                _trailler = value;
                OnPropertyChanged(nameof(SelectedTrailler));
            }
        }

        private string _windowName = "Добавление бренда прицепа";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public TraillerBrandViewModel()
        {
            mode = Mode.Additing;
            _brand = new TraillerBrand();
            Traillers = new ObservableCollection<Trailler>();
        }

        public TraillerBrandViewModel(TraillerBrand brand)
        {
            mode = Mode.Editing;

            _context.Entry(brand).Collection(b => b.Traillers).Load();            

            _brand = brand.Clone() as TraillerBrand;
            Traillers = new ObservableCollection<Trailler>(_brand.Traillers);
            WindowName = "Редактирование бренда";
        }

        protected override void setCommands()
        {
            AddTraillerCommand = new DelegateCommand((obj) => addTrailler());
            AddTraillerByKeyboardCommand = new DelegateCommand(addTraillerByKeyboard);
            GetTraillerSourceCommand = new DelegateCommand(getTraillers);
            DeleteCommand = new DelegateCommand(deleteTrailler);
        }

        private void addTrailler()
        {
            if (SelectedTrailler != null)
            {
                if (Traillers.Contains(SelectedTrailler)) MessageBox.Show($"Транспорт {SelectedTrailler.Number} уже числится за брендом {_brand.Name}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (!string.IsNullOrEmpty(Name))
                {
                    Traillers.Add(SelectedTrailler);
                    _brand.Traillers.Add(SelectedTrailler);
                    if (mode == Mode.Additing) SelectedTrailler.Brand = new TraillerBrand() { Name = this.Name };
                    else if (mode == Mode.Editing) SelectedTrailler.BrandId = _brand.TraillerBrandId;
                    SelectedTrailler = null;
                }
                else
                {
                    MessageBox.Show($"Неверно заполнено поле 'Наименование бренда' - {_brand.Name}. Чтобы добавить автомобиль, следует, сначала, корректно указать бренд.", "Укажите бренд", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void addTraillerByKeyboard(object e)
        {
            if ((Key)e == Key.Enter) addTrailler();
        }

        private void deleteTrailler(object obj)
        {
            _brand.Traillers.Remove(obj as Trailler);
            Traillers.Remove(obj as Trailler);
        }

        private void getTraillers(object obj)
        {
            string text = obj as string;
            if (!string.IsNullOrEmpty(text))
            {
                TraillerSource = _context.Traillers
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
                MessageBox.Show($"Неверно заполнено поле 'Наименование бренда' - {_brand.Name}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected override void addEntity()
        {
            _context.Add(_brand);
        }

        protected override void updateEntity()
        {
            var br = _context.TraillerBrands.Find(_brand.TraillerBrandId);
            br.SetFields(_brand);
        }

        public override ICloneable GetEntity() => _brand;
    }
}
