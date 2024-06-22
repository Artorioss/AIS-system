using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CreateTraillerViewModel: BaseViewModel
    {
        TransportationEntities _context;
        Trailler _trailler;
        public DelegateCommand GetBrands { get; private set; }
        public DelegateCommand CreateTrailler { get; private set; }

        Mode _mode;
        
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

        public TraillerBrand Brand
        {
            get => _trailler.Brand;
            set
            {
                _trailler.Brand = value;
                OnPropertyChanged(nameof(Brand));
            }
        }

        public string Number
        {
            get => _trailler.Number;
            set
            {
                _trailler.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private string _buttonText = "Добавить запись";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        private string _windowName = "Добавления трейлера";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public CreateTraillerViewModel(Trailler trailler)
        {
            settingsUp();
            _trailler = trailler.Clone() as Trailler;
            BrandSource = new List<TraillerBrand>() { trailler.Brand };
            ButtonText = "Обновить запись";
            WindowName = "Редактировать запись";
            _mode = Mode.Editing;   
        }

        public CreateTraillerViewModel()
        {
            settingsUp();
            _trailler = new Trailler();
            _context.Add(_trailler);
            _mode = Mode.Additing;
        }

        private void settingsUp() 
        {
            _context = (Application.Current as App)._context;
            GetBrands = new DelegateCommand(getBrands);
            CreateTrailler = new DelegateCommand(CreateTraillerAction);
        }


        private void getBrands(object obj)
        {
            string text = (obj as string).ToLower();
            BrandSource = _context.TraillerBrands.Where(c => c.Name.ToLower().Contains(text) || c.RussianName.ToLower().Contains(text))
                                         .Take(5)
                                         .Select(c => c)
                                         .ToList();
        }

        private void CreateTraillerAction(object obj)
        {
            if (Brand != null && !string.IsNullOrWhiteSpace(Number))
            {
                if (_mode == Mode.Editing)
                {
                    var existingTrailler = _context.Traillers.Find(_trailler.Number);
                    existingTrailler.Brand = _context.TraillerBrands.Find(Brand.TraillerBrandId);
                    existingTrailler.Number = Number;
                }
                _context.SaveChanges();
                (obj as Window)?.Close();
            }
            else
            {
                MessageBox.Show("Неправильно заполнены поля!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
