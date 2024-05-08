using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppMVVM.Models;

namespace WpfAppMVVM
{
    /// <summary>
    /// Логика взаимодействия для WindowReferencesBook.xaml
    /// </summary>
    public partial class WindowReferencesBook : Window
    {
        private TransportationEntities _context;
        private object _sender;
        public WindowReferencesBook()
        {
            InitializeComponent();
            _context = (Application.Current as App)._context;
            buttonCars_Click(null, null);
        }

        private void buttonCars_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnCarBrand = new DataGridTextColumn();
            columnCarBrand.Header = "Бренд";
            columnCarBrand.Binding = new Binding("CarBrand.Name");
            columnCarBrand.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridCheckBoxColumn columnIsTrack = new DataGridCheckBoxColumn();
            columnIsTrack.Header = "Тягач";
            columnIsTrack.Binding = new Binding("IsTruck");
            columnIsTrack.Width = new DataGridLength(50, DataGridLengthUnitType.Auto);


            dataGridView.Columns.Add(columnCarBrand);
            dataGridView.Columns.Add(columnNumber);
            dataGridView.Columns.Add(columnIsTrack);

            dataGridView.ItemsSource = _context.Cars.Include(car => car.CarBrand).ToList();
            _sender = sender;
        }

        private void buttonCarBrands_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnName);
            dataGridView.ItemsSource = _context.CarBrands.ToList();
        }

        private void buttonCustomers_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnName);
            dataGridView.ItemsSource = _context.Customers.ToList();
        }

        private void buttonDrivers_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnTransportCompany = new DataGridTextColumn();
            columnTransportCompany.Header = "Транспортная компания";
            columnTransportCompany.Binding = new Binding("TransportCompany.Name");
            columnTransportCompany.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnName);
            dataGridView.Columns.Add(columnTransportCompany);
            dataGridView.ItemsSource = _context.Drivers.Include(driver => driver.TransportCompany).ToList();
        }

        private void buttonRoutePoints_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnName);
            dataGridView.ItemsSource = _context.RoutePoints.ToList();
        }

        private void buttonRoutes_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnRouteName = new DataGridTextColumn();
            columnRouteName.Header = "Маршрут";
            columnRouteName.Binding = new Binding("RouteName");
            columnRouteName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnRouteName);
            dataGridView.ItemsSource = _context.Routes.ToList();
        }

        private void buttonRussinaBrandNames_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnRUBrandName = new DataGridTextColumn();
            columnRUBrandName.Header = "Русское название бренда";
            columnRUBrandName.Binding = new Binding("Name");
            columnRUBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование бренда";
            columnBrandName.Binding = new Binding("CarBrand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnRUBrandName);
            dataGridView.Columns.Add(columnBrandName);
            dataGridView.ItemsSource = _context.RussianBrandNames.Include(RuBrand => RuBrand.СarBrand).ToList();
        }

        private void buttonStateOrders_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnName = new DataGridTextColumn();
            columnName.Header = "Наименование";
            columnName.Binding = new Binding("Name");
            columnName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnName);
            dataGridView.ItemsSource = _context.StateOrders.ToList();
        }

        private void buttonTraillers_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Бренд";
            columnBrandName.Binding = new Binding("CarBrand.Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            DataGridTextColumn columnNumber = new DataGridTextColumn();
            columnNumber.Header = "Номер";
            columnNumber.Binding = new Binding("Number");
            columnNumber.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnBrandName);
            dataGridView.Columns.Add(columnNumber);
            dataGridView.ItemsSource = _context.Traillers.Include(trailer => trailer.CarBrand).ToList();
        }

        private void buttonTransportCompanies_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.Columns.Clear();

            DataGridTextColumn columnBrandName = new DataGridTextColumn();
            columnBrandName.Header = "Наименование";
            columnBrandName.Binding = new Binding("Name");
            columnBrandName.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dataGridView.Columns.Add(columnBrandName);
            dataGridView.ItemsSource = _context.TransportCompanies.ToList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
