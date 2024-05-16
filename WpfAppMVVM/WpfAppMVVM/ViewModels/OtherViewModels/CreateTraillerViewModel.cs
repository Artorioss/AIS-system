using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CreateTraillerViewModel: BaseViewModel
    {
        TransportationEntities _context;
        Trailler _trailler;
        public DelegateCommand GetBrands { get; private set; }
        public DelegateCommand CreateTrailler { get; private set; }

        private List<Brand> _brandSource;
        public List<Brand> BrandSource
        {
            get => _brandSource;
            set
            {
                _brandSource = value;
                OnPropertyChanged(nameof(BrandSource));
            }
        }

        public Brand Brand
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
            _trailler = trailler;
            BrandSource = new List<Brand>() { trailler.Brand };
            ButtonText = "Обновить запись";
            WindowName = "Редактировать запись";
        }

        public CreateTraillerViewModel()
        {
            settingsUp();
            _trailler = new Trailler();
            _context.Add(_trailler);
        }

        private void settingsUp() 
        {
            _context = (Application.Current as App)._context;
            GetBrands = new DelegateCommand(getBrands);
            CreateTrailler = new DelegateCommand(createTrailler);
        }


        private void getBrands(object obj)
        {
            string text = (obj as string).ToLower();
            BrandSource = _context.Brands.Where(c => c.Name.ToLower().Contains(text) || c.RussianBrandName.ToLower().Contains(text))
                                         .Take(5)
                                         .Select(c => c)
                                         .ToList();
        }

        private void createTrailler(object obj)
        {
            if (Brand != null && Number != null)
            {
                _context.SaveChanges();
                (obj as Window).Close();
            }
            else MessageBox.Show("Заполните все поля", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
