using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models.Entities;
using static WpfAppMVVM.Model.CustomComboBox;

namespace WpfAppMVVM.ViewModels
{
    internal class ReferenceBookViewModel : BaseViewModel
    {
        public DataGrid dataGrid { get; set; }
        public ObservableCollection<DataGridColumn> ColumnCollection { get; private set; }
        public ObservableCollection<object> ItemsSource { get; private set; }
        public DelegateCommand GetCarsData { get; private set; }
        public DelegateCommand GetCarBrandsData { get; private set; }
        public DelegateCommand GetCustomersData { get; private set; }
        public DelegateCommand GetDriversData { get; private set; }
        public DelegateCommand GetRoutePointsData { get; private set; }
        public DelegateCommand GetRoutesData { get; private set; }
        public DelegateCommand GetRussinaBrandNamesData { get; private set; }
        public DelegateCommand GetStateOrdersData { get; private set; }
        public DelegateCommand GetTraillersData { get; private set; }
        public DelegateCommand GetTransportCompaniesData { get; private set; }
        public DelegateCommand AddData { get; private set; }
        public ReferenceBookViewModel() 
        {
            ColumnCollection = new ObservableCollection<DataGridColumn>();
            ItemsSource = new ObservableCollection<object>();

            GetCarsData = new DelegateCommand((obj) => getCarsData());
            GetCarBrandsData = new DelegateCommand((obj) => getCarBrandsData());
            GetCustomersData = new DelegateCommand((obj) => getCustomersData());
            GetDriversData = new DelegateCommand((obj) => getDriversData());
            GetRoutePointsData = new DelegateCommand((obj) => getRoutePointsData());
            GetRoutesData = new DelegateCommand((obj) => getRoutesData());
            GetRussinaBrandNamesData = new DelegateCommand((obj) => getRussinaBrandNamesData());
            GetStateOrdersData = new DelegateCommand((obj) => getStateOrdersData());
            GetTraillersData = new DelegateCommand((obj) => getTraillersData());
            GetTransportCompaniesData = new DelegateCommand((obj) => getTransportCompaniesData());

            AddData = new DelegateCommand((obj) => addData());

            getCarsData();
        }

        private void LoadDataInCollection(IEnumerable query) 
        {
            ItemsSource.Clear();
            foreach (var items in query)
            {
                ItemsSource.Add(items);
            }
        }

        //DataGridTextColumn columnCarBrand = new DataGridTextColumn();
        //columnCarBrand.Header = "Бренд";
        //columnCarBrand.Binding = new Binding("CarBrand.Name");
        //columnCarBrand.Width = new DataGridLength(1, DataGridLengthUnitType.Star);


        private CarBrand _carBrand = new CarBrand();
        public CarBrand CarBrand 
        {
            get => _carBrand;
            set 
            {
                _carBrand = value;
                OnPropertyChanged(nameof(CarBrand));
            }
        }

        List<CarBrand> _carBrands = new List<CarBrand>();
        List<CarBrand> CarBrands 
        {
            get => _carBrands;
            set 
            {
                _carBrands = value;
                OnPropertyChanged(nameof(CarBrands));
            }
        }
        private void getMarks(object e) 
        {
            CarBrands = _context.CarBrands.Where(s => s.Name.Contains((string)e)).ToList();
        }

        private void getCarsData() 
        {
            ColumnCollection.Clear();
            var data = _context.Cars.Include(car => car.CarBrand);

            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn();
            dataGridTemplateColumn.Header = "Бренд";
            dataGridTemplateColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            FrameworkElementFactory comboBoxFactory = new FrameworkElementFactory(typeof(CustomComboBox));
            comboBoxFactory.SetValue(CustomComboBox.ItemsSourceProperty, CarBrands);
            comboBoxFactory.SetValue(CustomComboBox.SelectedItemProperty, CarBrand);
            comboBoxFactory.SetBinding(CustomComboBox.SelectedItemProperty, new Binding("Name"));


            // Создание обработчика события
            CustomEventHandler customEventHandler = new CustomEventHandler((obj,e) => getMarks(comboBoxFactory.Text));

            // Создание шаблона для ячейки колонки
            DataTemplate cellTemplate = new DataTemplate();
            comboBoxFactory.AddHandler(CustomComboBox.CustomEventHandler, customEventHandler); // Подключение обработчика события к элементу ComboBox


            dataGridTemplateColumn.CellTemplate = new DataTemplate() { VisualTree = comboBoxFactory };

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridCheckBoxColumn columnIsTrack = new DataGridCheckBoxColumn();
            columnIsTrack.Header = "Тягач";
            columnIsTrack.Binding = new Binding("IsTruck");
            columnIsTrack.Width = new DataGridLength(50, DataGridLengthUnitType.Auto);

            ColumnCollection.Add(dataGridTemplateColumn);
            ColumnCollection.Add(columnNumber);
            ColumnCollection.Add(columnIsTrack);

            
            LoadDataInCollection(data);
        }

        private void getCarBrandsData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            var data = _context.CarBrands;
            LoadDataInCollection(data);
        }

        private void getCustomersData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            var data = _context.Customers;
            LoadDataInCollection(data);
        }

        private void getDriversData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnTransportCompany = new DataGridTextColumn();
            columnTransportCompany.Header = "Транспортная компания";
            columnTransportCompany.Binding = new Binding("TransportCompany.Name");
            columnTransportCompany.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);
            ColumnCollection.Add(columnTransportCompany);

            var data = _context.Drivers.Include(driver => driver.TransportCompany);
            LoadDataInCollection(data);
        }

        private void getRoutePointsData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);

            var data = _context.RoutePoints;
            LoadDataInCollection(data);
        }

        private void getRoutesData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnRouteName = new DataGridTextColumn();
            columnRouteName.Header = "Маршрут";
            columnRouteName.Binding = new Binding("RouteName");
            columnRouteName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnRouteName);

            var data = _context.Routes;
            LoadDataInCollection(data);
        }

        private void getRussinaBrandNamesData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnRUBrandName = new DataGridTextColumn();
            columnRUBrandName.Header = "Русское название бренда";
            columnRUBrandName.Binding = new Binding("Name");
            columnRUBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование бренда";
            columnBrandName.Binding = new Binding("CarBrand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnRUBrandName);
            ColumnCollection.Add(columnBrandName);

            var data = _context.RussianBrandNames.Include(RuBrand => RuBrand.СarBrand);
            LoadDataInCollection(data);
        }

        private void getStateOrdersData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnName);

            var data = _context.StateOrders;
            LoadDataInCollection(data);
        }

        private void getTraillersData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Бренд";
            columnBrandName.Binding = new Binding("CarBrand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);
            ColumnCollection.Add(columnNumber);

            var data = _context.Traillers.Include(trailer => trailer.TraillerBrand);
            LoadDataInCollection(data);
        }

        private void getTransportCompaniesData()
        {
            ColumnCollection.Clear();

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование";
            columnBrandName.Binding = new Binding("Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            ColumnCollection.Add(columnBrandName);

            var data = _context.TransportCompanies;
            LoadDataInCollection(data);
        }

        private void addData() 
        {
            
        }
    }
}
