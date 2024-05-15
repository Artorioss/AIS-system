using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.Entities;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class CreateDriverViewModel: BaseViewModel
    {
        TransportationEntities _context;
        public DelegateCommand GetCustomers { get; private set; }
        public DelegateCommand CreateDriver { get; private set; }
        Driver _driver;

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

        public CreateDriverViewModel() 
        {
            settingsUp();
            _driver = new Driver();
            _context.Add(_driver);
        }

        public CreateDriverViewModel(Driver driver)
        {
            settingsUp();
            _driver = driver;
            TransportCompanySource = new List<TransportCompany>() { _driver.TransportCompany };
            ButtonText = "Обновить запись";
            WindowName = "Редактирование записи";
        }

        private void settingsUp() 
        {
            _context = (Application.Current as App)._context;
            GetCustomers = new DelegateCommand(getCustomers);
            CreateDriver = new DelegateCommand(createCustomer);
        }

        private void getCustomers(object obj)
        {
            string text = obj as string;
            TransportCompanySource = _context.TransportCompanies.Where(c => c.Name.ToLower().Contains(text.ToLower())).Take(5).Select(c => c).ToList();
        }

        private void createCustomer(object obj) 
        {
            if (TransportCompany != null && Name != null)
            {
                _context.SaveChanges();
                (obj as Window).Close();
            }
            else MessageBox.Show("Заполните все поля", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
