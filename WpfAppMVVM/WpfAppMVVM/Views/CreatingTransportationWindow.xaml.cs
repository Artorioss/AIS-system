using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;
using WpfAppMVVM.ViewModels;

namespace WpfAppMVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для CreatingTransportationWindow.xaml
    /// </summary>
    /// 
    public partial class CreatingTransportationWindow : Window
    {
        TransportationEntities TransportationEntities { get; set; }
        public Transportation _transportation { get; set; }
        private Driver _driver;
        private Car _car;
        private Trailler _trailer;
        private Customer _customer;
        private TransportCompany _transportCompany;

        private RoutePointLoader _routePointLoader;

        public CreatingTransportationWindow()
        {
            InitializeComponent();
            //TransportationEntities = transportationEntities;
            _routePointLoader = new RoutePointLoader();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _transportation = new Transportation
                {
                    //DateLoading = datePickerUpload.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    CustomerId = _customer.CustomerId,
                    DriverId = _driver.DriverId,
                    TransportCompanyId = _transportCompany.TransportCompanyId,
                    //Price = textBoxSum.Text is null ? null : Convert.ToDecimal(textBoxSum.Text),
                    //PaymentToDriver = textBoxPayment.Text is null ? null : Convert.ToDecimal(textBoxPayment.Text),
                    AccountNumber = null,
                    AccountDate = null,
                    //Address = textBoxGeneralRoute.Text,
                    StateOrderId = TransportationEntities.StateOrders.Where(state => state.Name == "Обработка").Select(state => state.StateOrderId).Single()
                };
                TransportationEntities.Transportations.Add(_transportation);
                TransportationEntities.SaveChanges();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось создать заявку - {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBoxLoading_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox_TextChanged(sender, e);
        }

        private void ComboBoxDispatcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox_TextChanged(sender, e);
        }

        //Точка загрузки и точка выгрузки
        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.OriginalSource;
            ComboBox comboBox = sender as ComboBox;
            if (textBox.SelectionStart == 0 && comboBox.SelectedItem == null)
            {
                comboBox.IsDropDownOpen = false;
            }
            else comboBox.SelectedItem = null;

            comboBox.ItemsSource = TransportationEntities.RoutePoints.AsNoTracking()
                                          .Where(c => c.Name.ToLower().Contains(textBox.Text.ToLower()))
                                          .OrderBy(c => c.Name)
                                          .Take(5)
                                          .ToList();


            comboBox.IsDropDownOpen = comboBox.Items.Count > 0;
        }

        //Заказчик
        private void comboBoxCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.OriginalSource;
            ComboBox comboBox = sender as ComboBox;
            if (textBox.SelectionStart == 0 && comboBox.SelectedItem == null)
            {
                comboBox.IsDropDownOpen = false;
            }
            else comboBox.SelectedItem = null;

            comboBox.ItemsSource = TransportationEntities.Customers.AsNoTracking()
                                        .Where(c => c.Name.ToLower().Contains(textBox.Text.ToLower()))
                                        .OrderBy(c => c.Name)
                                        .Take(5)
                                        .ToList();

            // Открываем выпадающий список, если есть результаты
            comboBox.IsDropDownOpen = comboBox.Items.Count > 0;
        }

        //Клиенты потеря фокуса
        private void comboBoxCustomers_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.IsFocused)
            {
                if (comboBox.SelectedItem != null)
                {
                    _customer = comboBox.SelectedItem as Customer;
                }
                else if (!string.IsNullOrEmpty(comboBox.Text))
                {
                    Customer cust = TransportationEntities.Customers
                           .FirstOrDefault(s => s.Name.ToLower() == comboBox.Text.ToLower());

                    if (cust is null)
                        _customer = new Customer { Name = comboBox.Text };
                    else
                    {
                        _customer = cust;
                        comboBox.Text = cust.Name;
                    }
                }
            }
        }

        //Водитель
        private void comboBoxDriver_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.OriginalSource;
            ComboBox comboBox = sender as ComboBox;
            if (textBox.SelectionStart == 0 && comboBox.SelectedItem == null)
            {
                comboBox.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, сбросить фокус выпадающего списка
            }
            else comboBox.SelectedItem = null;
            // Выполняем запрос к базе данных для каждого введенного значения
            comboBox.ItemsSource = TransportationEntities.Drivers.AsNoTracking()
                                          .Where(c => c.Name.ToLower().Contains(textBox.Text.ToLower()))
                                          .OrderBy(c => c.Name)
                                          .Take(5)
                                          .ToList();

            // Открываем выпадающий список, если есть результаты
            comboBox.IsDropDownOpen = comboBox.Items.Count > 0;
        }

        private void ComboboxDriver_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.IsFocused)
            {
                if (comboBox.SelectedItem != null)
                {
                    _driver = comboBox.SelectedItem as Driver;
                    setCompanyDriver();
                    trySetTransportByDriver();
                    trySetTraillerByDriver();
                }
                else if (!string.IsNullOrEmpty(comboBox.Text))
                {
                    Driver driver = TransportationEntities.Drivers
                           .FirstOrDefault(s => s.Name.ToLower().Contains(comboBox.Text.ToLower()));

                    if (driver is null)
                        _driver = new Driver { Name = comboBox.Text };
                    else
                    {
                        _driver = driver;
                        comboBox.Text = driver.Name;
                        setCompanyDriver();
                        trySetTransportByDriver();
                        trySetTraillerByDriver();
                    }
                }
            }
        }

        private void setCompanyDriver() 
        {
            //var driverCompany = TransportationEntities.TransportCompanies
            //    .Where(comp => comp.Drivers.Any(driver => driver.DriverId == _driver.DriverId))
            //    .FirstOrDefault();
            //comboBoxTransportCompany.Text = driverCompany.Name;
        }

        private void trySetTransportByDriver()
        {
            //var cars = TransportationEntities.Cars
            //    .Include(car => car.Drivers)
            //    .Where(car => car.Drivers.Any(driver => driver.DriverId == _driver.DriverId))
            //    .Include(car => car.CarBrand)
            //    .Select(car => car);

            //ComboBoxCarBrand.ItemsSource = cars.Select(car => car.CarBrand).ToList();
            //ComboBoxCarNumber.ItemsSource = cars.ToList();
            //if (cars.Count() == 1)
            //{
            //    _car = cars.Single();
            //    ComboBoxCarBrand.SelectedItem = _car.CarBrand;
            //    ComboBoxCarNumber.SelectedItem = _car;
            //}
            //else
            //{
            //    ComboBoxCarBrand.ItemsSource = cars.ToList();
            //    ComboBoxCarBrand.Text = null;
            //    ComboBoxCarNumber.Text = null;
            //}
        }

        private void trySetTraillerByDriver()
        {
            //var traillers = TransportationEntities.Traillers
            //    .Include(trailler => trailler.Drivers)
            //    .Include(trailler => trailler.CarBrand)
            //    .Where(trailler => trailler.Drivers.Any(driver => driver.DriverId == _driver.DriverId))
            //    .Select(trailler => trailler);


            //comboBoxTrailerBrand.ItemsSource = traillers.ToList();
            //comboBoxTrailerNumber.ItemsSource = traillers.ToList();
            //if (traillers.Count() == 1)
            //{
                //_trailer = traillers.Single();
                //comboBoxTrailerBrand.SelectedItem = _trailer;
                //comboBoxTrailerNumber.SelectedItem = _trailer;
            //}
            //else
            //{
                //comboBoxTrailerBrand.ItemsSource = traillers.ToList();
                //comboBoxTrailerBrand.Text = null;
                //comboBoxTrailerNumber.Text = null;
            //}
        }

        private void comboBoxCar_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.OriginalSource;
            ComboBox comboBox = sender as ComboBox;
            if (textBox.SelectionStart == 0 && comboBox.SelectedItem == null)
            {
                comboBox.IsDropDownOpen = false;
            }
            else comboBox.SelectedItem = null;

            if (comboBox.Text != null) 
            {
                var items = TransportationEntities.CarBrands
                    .AsNoTracking()
                    .Include(car => car.RussianBrandNames)
                    .Where(s => s.Name.ToLower().Contains(comboBox.Text.ToLower()) || s.RussianBrandNames.Count > 0 && s.RussianBrandNames.Any(ruName => ruName.Name.Contains(Name.ToLower())))
                    .OrderBy(s => s.Name)
                    .Select(s => s)
                    .Take(5)
                    .ToList();

                comboBox.ItemsSource = items; 
            }

            comboBox.IsDropDownOpen = comboBox.Items.Count > 0;
        }

        private void textBoxGeneralRoute_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (textBoxGeneralRoute.IsFocused)
            //{
            //    _routePointLoader.setRoutePoints(textBoxGeneralRoute.Text);
            //}
        }

        private void buttonAddRouteDispatcher_Click(object sender, RoutedEventArgs e)
        {
            //if (comboBoxRouteDispatcher.SelectedItem != null)
            //{
            //    _routePointLoader.AddDispatcher(comboBoxRouteDispatcher.SelectedItem as RoutePoint);
            //    comboBoxRouteDispatcher.SelectedItem = null;
            //    textBoxGeneralRoute.Text = _routePointLoader.ToString();
            //    comboBoxRouteDispatcher.IsDropDownOpen = false;
            //}
            //else if (comboBoxRouteDispatcher.Text != string.Empty)
            //{
            //    RoutePoint route_Point = TransportationEntities.RoutePoints.Where(s => s.Name.ToLower().Contains(comboBoxRouteDispatcher.Text.ToLower())).FirstOrDefault();
            //    if (route_Point is null)
            //        route_Point = new RoutePoint { Name = comboBoxRouteDispatcher.Text };
            //    _routePointLoader.AddDispatcher(route_Point);
            //    textBoxGeneralRoute.Text = _routePointLoader.ToString();
            //    comboBoxRouteDispatcher.Text = string.Empty;
            //}
        }

        private void buttonAddRouteLoading_Click(object sender, RoutedEventArgs e)
        {
            //if (comboBoxRouteLoading.SelectedItem != null)
            //{
            //    _routePointLoader.AddLoading(comboBoxRouteLoading.SelectedItem as RoutePoint);
            //    comboBoxRouteLoading.SelectedItem = null;
            //    textBoxGeneralRoute.Text = _routePointLoader.ToString();
            //    comboBoxRouteLoading.IsDropDownOpen = false;
            //}
            //else if (comboBoxRouteLoading.Text != string.Empty)
            //{
            //    RoutePoint route_Point = TransportationEntities.RoutePoints.Where(s => s.Name.ToLower().Contains(comboBoxRouteLoading.Text.ToLower())).FirstOrDefault();
            //    if (route_Point is null)
            //        route_Point = new RoutePoint { Name = comboBoxRouteLoading.Text };
            //    _routePointLoader.AddLoading(route_Point);
            //    textBoxGeneralRoute.Text = _routePointLoader.ToString();
            //    comboBoxRouteLoading.Text = string.Empty;
            //}
        }

        private void comboBoxRouteLoading_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) buttonAddRouteLoading_Click(sender, e);
        }

        private void comboBoxRouteDispatcher_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) buttonAddRouteDispatcher_Click(sender, e);
        }

        private void textBoxPayment_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if(!string.IsNullOrEmpty(textBoxPayment.Text) && !string.IsNullOrEmpty(textBoxSum.Text))
            //    textBoxDelta.Text = (Convert.ToDecimal(textBoxPayment.Text) - Convert.ToDecimal(textBoxSum.Text)).ToString();
        }

        private void comboBoxTransportCompany_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.IsFocused)
            {
                if (comboBox.SelectedItem != null)
                {
                    _transportCompany = comboBox.SelectedItem as TransportCompany;
                }
                else if (!string.IsNullOrEmpty(comboBox.Text))
                {
                    TransportCompany tc = TransportationEntities.TransportCompanies
                           .FirstOrDefault(s => s.Name.ToLower() == comboBox.Text.ToLower());

                    if (tc is null)
                        _transportCompany = new TransportCompany { Name = comboBox.Text };
                    else
                    {
                        _transportCompany = tc;
                        comboBox.Text = tc.Name;
                    }
                }
            }
        }

        private void comboBoxTransportCompany_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)e.OriginalSource;
            ComboBox comboBox = sender as ComboBox;
            if (textBox.SelectionStart == 0 && comboBox.SelectedItem == null)
            {
                comboBox.IsDropDownOpen = false;
            }
            else comboBox.SelectedItem = null;

            comboBox.ItemsSource = TransportationEntities.TransportCompanies.AsNoTracking()
                                        .Where(c => c.Name.ToLower().Contains(textBox.Text.ToLower()))
                                        .OrderBy(c => c.Name)
                                        .Take(5)
                                        .ToList();

            comboBox.IsDropDownOpen = comboBox.Items.Count > 0;
        }

        private void textBoxSum_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(textBoxPayment.Text) && !string.IsNullOrEmpty(textBoxSum.Text))
            //    textBoxDelta.Text = (Convert.ToDecimal(textBoxPayment.Text) - Convert.ToDecimal(textBoxSum.Text)).ToString();
        }
    }
}