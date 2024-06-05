using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CreateCarViewModel : BaseViewModel
    {
        public DelegateCommand CreateCar { get; private set; }
        public DelegateCommand GetBrands { get; private set; }

        TransportationEntities _context;
        Car _car;

        Mode _mode;

        List<Brand> _brandSource;
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
            get => _car.Brand;
            set 
            {
                _car.Brand = value;
                OnPropertyChanged(nameof(Brand));
            }
        }

        public string Number
        {
            get => _car.Number;
            set 
            {
                _car.Number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public bool IsTruck
        {
            get => _car.IsTruck;
            set
            {
                _car.IsTruck = value;
                OnPropertyChanged(nameof(IsTruck));
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

        private string _windowName = "Добавления автомобиля";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public CreateCarViewModel() 
        {
            settingsUp();
            _car = new Car();
            _context.Add(_car);
            _mode = Mode.Additing;
        }

        public CreateCarViewModel(Car car)
        {
            settingsUp();
            _car = (Car)car.Clone();
            BrandSource = new List<Brand>() { _car.Brand };
            ButtonText = "Обновить запись";
            WindowName = "Редактирование записи";
            _mode = Mode.Editing;
        }

        private void settingsUp() 
        {
            _context = (Application.Current as App)._context;
            CreateCar = new DelegateCommand(CreateCarAction);
            GetBrands = new DelegateCommand(getBrands);
        }

        private void getBrands(object obj) 
        {
            string text = obj as string;
            BrandSource = _context.Brands.Where(b => b.Name.ToLower().Contains(text.ToLower()) || b.RussianBrandName.ToLower().Contains(text.ToLower())).Take(5)
                                         .Select(s => s)
                                         .ToList();
        }

        private void CreateCarAction(object obj)
        {
            if (Brand != null && !string.IsNullOrWhiteSpace(Number))
            {
                if (_mode == Mode.Editing)
                {
                    var existingCar = _context.Cars.Find(_car.Number);

                    existingCar.Brand = _context.Brands.Find(Brand.BrandId);
                    existingCar.Number = Number;
                    existingCar.IsTruck = IsTruck;
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
